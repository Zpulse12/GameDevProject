using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Scripts.Managers;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Monster1: Enemy
    {



        private bool movingDown;

        private readonly int maxXDisplacment = (int)(12 * MapGenerator.tileSize * GameManager.gameScale);
        private readonly int maxYDisplacment = (int)(3 * MapGenerator.tileSize * GameManager.gameScale);
        private static readonly Rectangle HitboxData = new Rectangle(0, (int)(7 * GameManager.gameScale),
                                    (int)(37 * GameManager.gameScale), (int)(22 * GameManager.gameScale));

        public Monster1(Vector2 position) : base(position, HitboxData)
        {
            monsterFrameCount = 4;
            frameSize = new Vector2(43, 37);
        }


        public override void Update(float deltaTime, Vector2 knightPos)
        {
            if (movingDown)
            {
                if (displacement.Y >= maxYDisplacment)
                    movingDown = false;
                else
                    displacement.Y += velocity.Y * deltaTime;
            }
            else
            {
                if (displacement.Y <= 0)
                    movingDown = true;
                else
                    displacement.Y -= velocity.Y * deltaTime;
            }
            if (movingLeft)
            {
                if (displacement.X <= -maxXDisplacment)
                    movingLeft = false;
                else
                    displacement.X -= velocity.X * deltaTime;
            }
            else
            {
                if (displacement.X >= 0)
                    movingLeft = true;
                else
                    displacement.X += velocity.X * deltaTime;
            }
            hitbox.Update(position + displacement);
            if (timer >= animationDuration)
            {
                currentFrame.X = currentFrame.X + 1 >= monsterFrameCount ? 0 : currentFrame.X + 1;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MonstersManager.Monster1Texture, position + displacement, sourceRect, Color.White, 0, Vector2.Zero, 0.75f * GameManager.gameScale, spriteEffects, 0);
        }
    }
}
