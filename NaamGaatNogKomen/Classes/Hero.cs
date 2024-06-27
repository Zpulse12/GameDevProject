using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Input;
using System.Diagnostics;

namespace NaamGaatNogKomen.Classes
{
    public class Hero
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
        private int _screenWidth;
        private int _screenHeight;
        private int lastMovementDirection = 1;
        private bool isJumping = false;
        public bool isFalling = true;
        private float jumpSpeed = -12f;
        private float gravity = 0.5f;
        private float scale = 1.0f; // Ensure the hero is correctly scaled
        private float verticalSpeed = 0;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;

            if (isMoving && inputReader.ReadInput().X == 1)
                lastMovementDirection = 1;
            else if (isMoving && inputReader.ReadInput().X == -1)
                lastMovementDirection = -1;

            Texture2D textureToDraw = isMoving ? walkTexture : idleTexture;
            SpriteEffects spriteEffect = (lastMovementDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(textureToDraw, position, currentAnimation?.CurrentFrame?.SourceRectangle, Color.White, 0, new Vector2(), scale, spriteEffect, 0);
        }
        public void Update(GameTime gameTime)
        {
            ApplyGravity();
            Move();
            Animation currentAnimation = isMoving ? walkAnimation : idleAnimation;
            currentAnimation?.Update(gameTime);

            Debug.WriteLine($"Hero Position after Update: {position}");
        }


        private void ApplyGravity()
        {
            if (isFalling)
            {
                verticalSpeed += gravity;
                position.Y += verticalSpeed;
            }
            else
            {
                verticalSpeed = 0;
            }
            Debug.WriteLine($"Vertical speed: {verticalSpeed}, Position Y: {position.Y}");
        }

        public void StopFalling()
        {
            isFalling = false;
            verticalSpeed = 0;
            Debug.WriteLine("Hero stopped falling.");
        }

        private void Move()
        {
            Vector2 direction = inputReader.ReadInput();
            if (position.X < 0)
                position.X = 0;
            else if (position.X > _screenWidth - (walkTexture.Width / 8) * scale)
                position.X = _screenWidth - (walkTexture.Width / 8) * scale;
            else
            {
                if (direction.X == 0)
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

        public Rectangle GetBounds()
        {
            var bounds = new Rectangle((int)position.X, (int)position.Y, (int)((walkTexture.Width / 8) * scale), (int)(walkTexture.Height * scale));
            Debug.WriteLine($"Hero Bounds: {bounds}");
            return bounds;
        }


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
