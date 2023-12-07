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
        private Texture2D jumpTexture;
        private Animation walkAnimation;
        private Animation idleAnimation;
        private Animation jumpAnimation;
        private Vector2 position = new Vector2(100, 800);
        private bool isMoving = false;
        private Vector2 speed = new Vector2(0, 0);
        private Vector2 acceleration = new Vector2(0.001f, 1f);
        private bool isJumping = false;
        private float jumpSpeed = -15f;
        private float gravity = 0.5f;
        private int collisionLeft = 48;
        private int collisionRight = 144;
        private int collisionFloor = 300;
        private IInputReader inputReader;
        private int _screenWidth; 
        private int _screenHeight;
        private int lastMovementDirection = 1;

        public Hero(Texture2D walkTexture, Texture2D idleTexture,Texture2D jumpTexture, IInputReader inputReader)
        {
            this.walkTexture = walkTexture;
            this.idleTexture = idleTexture;
            this.jumpTexture = jumpTexture;
            this.inputReader = inputReader;

            walkAnimation = new Animation();
            walkAnimation.GetFramesFromTexture(walkTexture.Width, walkTexture.Height, 8, 1);

            idleAnimation = new Animation();
            idleAnimation.GetFramesFromTexture(idleTexture.Width, idleTexture.Height, 5, 1);

            jumpAnimation = new Animation();
            jumpAnimation.GetFramesFromTexture(jumpTexture.Width, jumpTexture.Height, 3, 1);
        }
        public void SetScreenSize(int screenWidth, int screenHeight) 
        { 
            this._screenWidth = screenWidth; 
            this._screenHeight = screenHeight; 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isJumping)
            {
                spriteBatch.Draw(jumpTexture, position, jumpAnimation.CurrentFrame.SourceRectangle, Color.White, 0, new Vector2(), 3, SpriteEffects.None, 0);
            }
            else
            {
                Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;

                if (isMoving && inputReader.ReadInput().X == 1)
                    lastMovementDirection = 1;
                else if (isMoving && inputReader.ReadInput().X == -1)
                    lastMovementDirection = -1;

                Texture2D textureToDraw = isMoving ? walkTexture : idleTexture;
                SpriteEffects spriteEffect = (lastMovementDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(textureToDraw, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White, 0, new Vector2(), 3, spriteEffect, 0);
            }
        }
        public void Update(GameTime gameTime)
        {
            Move();
            if (isJumping)
            {
                speed.Y += gravity;
                position.Y += speed.Y;
                jumpAnimation.Update(gameTime);
            }
            else
            {
                Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;
                currentAnimation.Update(gameTime);
            }
        }
        private void Move()
        {
            Vector2 direction = inputReader.ReadInput();
            if (direction.Y==1 && !isJumping)
            {
                isJumping = true;
                speed.Y = jumpSpeed;
            }
            if (isJumping)
            {
                speed.Y += gravity;
                position.Y += speed.Y;

                if (position.Y > _screenHeight - collisionFloor)
                {
                    isJumping = false;
                    position.Y = _screenHeight - collisionFloor;
                    speed.Y = 0;
                }
            }
            if (position.X < 0 - collisionLeft) //collision with left side of screen
                position.X = 0 - collisionLeft;
            else if (position.X > _screenWidth - collisionRight) //collision with right side of screen
                position.X = _screenWidth - collisionRight;
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
                    speed = Accelerate(speed, acceleration, -3, 6);
                    direction *= speed;
                    position += direction;
                    acceleration += new Vector2(0.005f, 1f);
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
