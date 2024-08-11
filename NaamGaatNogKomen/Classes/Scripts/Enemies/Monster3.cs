using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Scripts.Hero;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Monster3:Enemy
    {
        public Projectile projectile;


        private static readonly Rectangle HitboxData = new Rectangle(0, (int)(7 * GameManager.gameScale),
                                            (int)(37 * GameManager.gameScale), (int)(30 * GameManager.gameScale));

        public Monster3(Vector2 position) : base(position, HitboxData)
        {
            frameSize = new Vector2(48, 55);
            monsterFrameCount = 8;

            projectile = new Projectile(position);
        }


        public override void Update(float deltaTime, Vector2 knightPos)
        {
            // movement handling

            //// Left/Right movement
            float pos_X = position.X + frameSize.X / 2 * GameManager.gameScale;
            float knightPos_x = knightPos.X + Knight.knightSize.X / 2 * GameManager.gameScale;
            if (System.Math.Abs(knightPos_x - pos_X) > velocity.X * deltaTime * 2 && System.Math.Abs(knightPos_x - pos_X) < 3 * MapGenerator.tileSize * GameManager.gameScale)
            {
                if (knightPos_x - pos_X < 2) // go right
                {
                    movingLeft = true;
                    position.X -= velocity.X * deltaTime;
                }
                else
                {
                    movingLeft = false;
                    position.X += velocity.X * deltaTime;
                }
            }

            hitbox.Update(position);


            // animation handling
            if (timer >= animationDuration) // time interval between frames
            {
                currentFrame.X = currentFrame.X + 1 >= monsterFrameCount / 2 ? 0 : currentFrame.X + 1;

                if (currentFrame.X == 0)
                    currentFrame.Y = currentFrame.Y + 1 >= 2 ? 0 : currentFrame.Y + 1;

                timer = 0;
            }
            timer += deltaTime;

            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * (frameSize.X + 1)),
                                (int)(1 + currentFrame.Y * (frameSize.Y + 1)),
                                (int)frameSize.X, (int)frameSize.Y);

            if (!movingLeft)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;

            if (projectile.Disapeared())
            {
                if (movingLeft)
                    projectile = new Projectile(position + new Vector2(6 * GameManager.gameScale, 21 * GameManager.gameScale));
                else
                    projectile = new Projectile(position + new Vector2(16 * GameManager.gameScale, 21 * GameManager.gameScale));
            }

            projectile.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            projectile.Draw(spriteBatch, MonstersManager.Monster3ProjectileTexture);
            spriteBatch.Draw(MonstersManager.Monster3Texture, position + displacement, sourceRect, Color.White, 0, Vector2.Zero, 0.75f * GameManager.gameScale, spriteEffects, 0);
        }

        public void MoveLeft(int amount)
        {
            position.X -= amount;
            projectile.MoveLeft(amount);
        }
    }
}
