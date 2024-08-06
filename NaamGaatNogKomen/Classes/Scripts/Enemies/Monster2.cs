using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Monster2
    {
        public Hitbox hitbox;

        private float timer;

        private bool movingLeft;
        private Vector2 position;
        private Vector2 displacment;

        private Vector2 currentFrame;
        private Rectangle sourceRect;
        public SpriteEffects spriteEffects;


        private readonly int maxXDisplacment = (int)(6 * MapGenerator.tileSize * GameManager.gameScale);
        private readonly int MonsterFrameCount = 4;
        private readonly float animationDuration = 0.2f;
        private readonly Vector2 frameSize = new Vector2(32, 16); // w, h
        private readonly Vector2 velocity = new Vector2(25 * GameManager.gameScale, 0);

        public Monster2(Vector2 position)
        {
            movingLeft = true;
            this.position = position;
            displacment = Vector2.Zero;

            timer = 0;
            currentFrame = Vector2.Zero;

            hitbox = new Hitbox(new Rectangle((int)(4 * GameManager.gameScale), (int)(1 * GameManager.gameScale), (int)(24 * GameManager.gameScale), (int)(15 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);
        }


        public void Update(float deltaTime)
        {
            if (movingLeft)
            {
                if (displacment.X <= -maxXDisplacment)
                    movingLeft = false;
                else
                    displacment.X -= velocity.X * deltaTime;
            }
            else
            {
                if (displacment.X >= 0)
                    movingLeft = true;
                else
                    displacment.X += velocity.X * deltaTime;
            }
            hitbox.Update(position + displacment);

            if (timer >= animationDuration)
            {
                currentFrame.X = currentFrame.X + 1 >= MonsterFrameCount ? 0 : currentFrame.X + 1;
                timer = 0;
            }
            timer += deltaTime;

            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * (frameSize.X + 1)),
                                0,
                                (int)frameSize.X,
                                (int)frameSize.Y);

            if (!movingLeft)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, position + displacment, sourceRect, Color.White, 0, Vector2.Zero, 1f * GameManager.gameScale, spriteEffects, 0);
        }

        public void MoveLeft(int amount)
        {
            position.X -= amount;
        }
    }
}
