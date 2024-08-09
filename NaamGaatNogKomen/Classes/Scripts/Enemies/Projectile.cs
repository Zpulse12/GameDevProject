using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Projectile
    {
        public Hitbox hitbox;

        private float timer; //this is a timer used for the coins animation

        private Vector2 position;

        private Vector2 currentFrame; //this is the current frame the animation is using from the sprite sheet
        private Rectangle sourceRect; //this is used to select a portion of the spritesheet

        private readonly int FrameCount = 3;
        private readonly float animationDuration = 0.15f; //this is the time interval between frames of the animation
        private readonly Vector2 frameSize = new Vector2(16, 25); // w, h
        private readonly float gravity = 1 * GameManager.gameScale;
        private Vector2 velocity = new Vector2(0, 0);

        public Projectile(Vector2 position)
        {
            this.position = position;

            timer = 0;
            currentFrame = Vector2.Zero;
            velocity.Y = 0;

            hitbox = new Hitbox(new Rectangle(0, (int)(9 * GameManager.gameScale), (int)(16 * GameManager.gameScale), (int)(14 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);
        }

        public void Update(float deltaTime)
        {
            if (timer >= animationDuration) // time interval between frames
            {
                currentFrame.X = currentFrame.X + 1 >= FrameCount ? 0 : currentFrame.X + 1;

                timer = 0;
            }
            timer += deltaTime;

            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * (frameSize.X + 1)),
                                0, (int)frameSize.X, (int)frameSize.Y);

            velocity.Y += gravity * GameManager.gameScale * deltaTime;
            position.Y += velocity.Y;
            hitbox.Update(position);
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, Vector2.Zero, 0.75f * GameManager.gameScale, SpriteEffects.None, 0);
        }

        public void MoveLeft(int amount)
        {
            position.X -= amount;
        }

        public bool Disapeared()
        {
            return position.Y >= GameManager.mapHeight;
        }
    }
}
