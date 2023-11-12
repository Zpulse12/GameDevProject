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
        private Texture2D texture;
        Animation animation;
        private Vector2 position = new Vector2(0, 0);
        private Vector2 speed = new Vector2(0, 0);
        private Vector2 acceleration = new Vector2(0.0005f, 0.001f);
        private IInputReader inputReader;

        public Hero(Texture2D texture,IInputReader inputReader)
        {
            this.texture = texture;
            this.inputReader = inputReader;
            animation = new Animation();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 8, 1);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, animation.CurrentFrame.SourceRectangle, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            Move();
            animation.Update(gameTime);
        }
        private void Move()
        {
            Vector2 direction = inputReader.ReadInput();
            if (position.X < 0 - 16)
                position.X = 0 -16;
            else if (position.X > 800 - 48)
                position.X = 800 - 48;
            else
            {
                if (direction.X == 0)
                {
                    speed.X = 0;
                    acceleration.X = 0.0005f;
                }
                speed = Accelerate(speed, acceleration, -3, 3);
                direction *= speed;
                position += direction;
                acceleration += new Vector2(0.0005f,0.001f);
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
