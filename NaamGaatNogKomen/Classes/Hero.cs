using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes
{
    internal class Hero : IGameObject
    {
        private Texture2D walkTexture;
        private Texture2D idleTexture;
        private Animation walkAnimation;
        private Animation idleAnimation;
        private bool isMoving = false;
        private Vector2 position = new Vector2(0, 0);
        private Vector2 speed = new Vector2(0, 0);
        private Vector2 acceleration = new Vector2(0.001f, 1f);
        private IInputReader inputReader;

        public Hero(Texture2D walkTexture,Texture2D idleTexture, IInputReader inputReader)
        {
            this.walkTexture = walkTexture;
            this.idleTexture = idleTexture;
            this.inputReader = inputReader;

            walkAnimation = new Animation();
            walkAnimation.GetFramesFromTexture(walkTexture.Width, walkTexture.Height, 8, 1);

            idleAnimation = new Animation();
            idleAnimation.GetFramesFromTexture(idleTexture.Width, idleTexture.Height, 5, 1);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;
            if (isMoving == true && inputReader.ReadInput().X == 1)
                spriteBatch.Draw(walkTexture, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White);
            else if (isMoving == true && inputReader.ReadInput().X == -1)
                spriteBatch.Draw(walkTexture, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White, 0, new Vector2(), 25, SpriteEffects.FlipHorizontally,0);
            else
                spriteBatch.Draw(idleTexture, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            Move();
            Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;
            currentAnimation.Update(gameTime);
        }
        private void Move()
        {
            Vector2 direction = inputReader.ReadInput();
            if (position.X < 0 - 16) //collision with left side of screen
                position.X = 0 -16;
            else if (position.X > 800 - 48) //collision with right side of screen
                position.X = 800 - 48;
            else
            {
                if (direction.X == 0) //reset speed and acceleration when hero stops moving || changes direction
                {
                    speed.X = 0;
                    acceleration.X = 0.0005f;
                    isMoving = false;
                }
                else
                {
                    isMoving = true;
                    speed = Accelerate(speed, acceleration, -3, 3);
                    direction *= speed;
                    position += direction;
                    acceleration += new Vector2(0.001f, 1f);
                }
            }
        }

        private Vector2 Accelerate(Vector2 currentspeed, Vector2 acceleration, float minSpeed, float maxSpeed)
        {
            Vector2 newSpeed = currentspeed + acceleration;
            if (newSpeed.X < minSpeed)
                newSpeed.X = minSpeed;
            if (newSpeed.X > maxSpeed)
                newSpeed.X = maxSpeed;
            return newSpeed;
        }
    }
}
