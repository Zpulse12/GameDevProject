using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class Monster2: Enemy
    {
     
        private bool isAlive;


        private readonly int maxXDisplacment = (int)(6 * MapGenerator.tileSize * GameManager.gameScale);
        private readonly int monsterDeathFrameCount = 8;
        private readonly float deathAnimationDuration = 0.2f; //this is the time interval between frames of the animation
        private readonly Vector2 deathFrameSize = new Vector2(42, 16); // w, h
        private static readonly Rectangle HitboxData = new Rectangle((int)(4 * GameManager.gameScale), (int)(1 * GameManager.gameScale),
                            (int)(24 * GameManager.gameScale), (int)(15 * GameManager.gameScale));

        public Monster2(Vector2 position) : base(position, HitboxData)
        {
            isAlive = true;
            monsterFrameCount = 4;
            frameSize = new Vector2(32, 16);
        }


        public override void Update(float deltaTime, Vector2 knightPos)
        {
            if (isAlive)
            {
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
            }


            if (isAlive)
            {
                if (timer >= animationDuration) // time interval between frames
                {
                    currentFrame.X = currentFrame.X + 1 >= monsterFrameCount / 2 ? 0 : currentFrame.X + 1;
                    timer = 0;
                }
                timer += deltaTime;

                sourceRect = new Rectangle((int)(1 + currentFrame.X * (frameSize.X + 1)),
                                            0, (int)frameSize.X, (int)frameSize.Y);
            }

            else
            {
                if (timer >= deathAnimationDuration && currentFrame.Y != -1) // time interval between frames
                {
                    currentFrame.X = currentFrame.X + 1 >= monsterDeathFrameCount / 2 ? 0 : currentFrame.X + 1;

                    if (currentFrame.X == 0)
                        currentFrame.Y = currentFrame.Y == 1 ? -1 : currentFrame.Y + 1;
                    timer = 0;
                }
                timer += deltaTime;

                sourceRect = new Rectangle((int)(1 + currentFrame.X * (deathFrameSize.X + 1)),
                                           1 + (int)(currentFrame.Y) * (int)(deathFrameSize.Y + 1),
                                           (int)deathFrameSize.X, (int)deathFrameSize.Y);
            }

            if (!movingLeft)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
                spriteBatch.Draw(MonstersManager.Monster2Texture, position + displacement, sourceRect,
                    Color.White, 0, Vector2.Zero, 1f * GameManager.gameScale, spriteEffects, 0);
            else if (currentFrame.X != -1)
                spriteBatch.Draw(MonstersManager.Monster2DeathTexture, position + displacement, sourceRect,
                                    Color.White, 0, Vector2.Zero, 1f * GameManager.gameScale, spriteEffects, 0);
        }
        public void Die()
        {
            isAlive = false;
            currentFrame = Vector2.Zero;
            timer = 0;
            if (movingLeft)
                position.X -= 12 * GameManager.gameScale;
            else
                position.X += 12 * GameManager.gameScale;
        }
        public bool IsAlive()
        {
            return isAlive;
        }
        public void MoveLeft(int amount)
        {
            position.X -= amount;
        }
    }
}
