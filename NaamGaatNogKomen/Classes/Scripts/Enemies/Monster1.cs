using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Monster1
    {
        public Hitbox hitbox;

        private float timer; 

        private bool movingLeft;
        private bool movingDown;
        private Vector2 position;
        private Vector2 currentFrame; 
        private Rectangle sourceRect; 
        public SpriteEffects spriteEffects; 



        private readonly int MonsterFrameCount = 4;
        private readonly float animationDuration = 0.2f;
        private readonly Vector2 frameSize = new Vector2(43, 37); // w, h
        private readonly Vector2 velocity = new Vector2(4 * GameManager.gameScale, 4 * GameManager.gameScale);

        public Monster1()
        {
            movingLeft = true;
            movingDown = true;
            position = new Vector2(50, 50);

            timer = 0;
            currentFrame = Vector2.Zero;

            hitbox = new Hitbox(new Rectangle(0, (int)(7 * GameManager.gameScale), (int)(37 * GameManager.gameScale), (int)(30 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);
        }


        public void Update(float deltaTime)
        {
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
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, Vector2.Zero, 0.75f * GameManager.gameScale, spriteEffects, 0);
        }

        public void MoveLeft(int amount)
        {
            position.X -= amount;
        }
    }
}
