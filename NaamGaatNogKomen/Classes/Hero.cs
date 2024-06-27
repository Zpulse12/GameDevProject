using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Input;
using NaamGaatNogKomen;
using System.Collections.Generic;
using System.Diagnostics;

namespace NaamGaatNogKomen
{
    public class Hero
    {
        private Texture2D walkTexture;
        private Texture2D idleTexture;
        private Animation walkAnimation;
        private Animation idleAnimation;
        private bool isMoving = false;
        private Vector2 position = new Vector2(100, 300);
        private Vector2 speed = new Vector2(0, 0);
        private float jumpSpeed = -12f;
        private float gravity = 0.5f;
        private float verticalSpeed = 0;
        private bool isFalling = true;
        private int lastMovementDirection = 1;
        private float scale = 1.0f;
        private IInputReader inputReader;
        private int _screenWidth;
        private int _screenHeight;

        public bool IsFalling
        {
            get { return isFalling; }
            set { isFalling = value; }
        }
        public float VerticalSpeed
        {
            get { return verticalSpeed; }
            set { verticalSpeed = value; }
        }
        public Hero(Texture2D walkTexture, Texture2D idleTexture, IInputReader inputReader)
        {
            this.walkTexture = walkTexture;
            this.idleTexture = idleTexture;
            this.inputReader = inputReader;

            walkAnimation = new Animation();
            walkAnimation.GetFramesFromTexture(walkTexture.Width, walkTexture.Height, 8, 1);

            idleAnimation = new Animation();
            idleAnimation.GetFramesFromTexture(idleTexture.Width, idleTexture.Height, 5, 1);
        }

        public void SetScreenSize(int screenWidth, int screenHeight, int tileHeight)
        {
            this._screenWidth = screenWidth;
            this._screenHeight = screenHeight;
        }

        public void StopFalling()
        {
            IsFalling = false;
            verticalSpeed = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;
            SpriteEffects spriteEffect = (lastMovementDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(isMoving ? walkTexture : idleTexture, position, currentAnimation.CurrentFrame, Color.White, 0, Vector2.Zero, scale, spriteEffect, 0);
        }

        public void Update(GameTime gameTime, List<Rectangle> platforms)
        {
            ApplyGravity(platforms);
            Move();
            Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;
            currentAnimation.Update(gameTime);

            Debug.WriteLine($"Hero Position after Update: {position}");
        }

        private void Move()
        {
            Vector2 direction = inputReader.ReadInput();
            if (direction.X != 0)
            {
                position.X += direction.X * 5f;
                isMoving = true;
                lastMovementDirection = (int)direction.X;
            }
            else
            {
                isMoving = false;
            }

            if (direction.Y != 0 && !IsFalling)
            {
                verticalSpeed = jumpSpeed;
                IsFalling = true;
            }
        }

        private void ApplyGravity(List<Rectangle> platforms)
        {
            verticalSpeed += gravity;
            position.Y += verticalSpeed;
            IsFalling = true;

            Rectangle heroBounds = GetBounds();
            foreach (var platform in platforms)
            {
                if (heroBounds.Intersects(platform))
                {
                    if (verticalSpeed > 0) // Falling down
                    {
                        position.Y = platform.Y - heroBounds.Height;
                        IsFalling = false;
                        verticalSpeed = 0;
                    }
                }
            }

            Debug.WriteLine($"Vertical speed: {verticalSpeed}, Position Y: {position.Y}");
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(walkTexture.Width / 8 * scale), (int)(walkTexture.Height * scale));
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
