﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Hero
{
    internal class KnightAnimation
    {
        public Vector2 position;
        public SpriteEffects spriteEffects;
        private float timer;
        private Texture2D texture;
        private Vector2 currentFrame;
        private Rectangle sourceRect;
        private KnightMovementStates prevMovementState;
        private readonly int[] framesCount = new int[] { 5, 8, 1, 1, 7 };
        public readonly float[] animationDuration = new float[] { 0.175f, 0.075f, 1f, 1f, 0.4f }; //this is the full duration of the animation
        private readonly int[][] frameSize = new int[][] { new int[] { 21, 22 }, new int[] { 21, 22 }, new int[] { 21, 22 }, new int[] { 21, 22 }, new int[] { 44, 27 } };
        private readonly int maxFrameHight = 23;
        public KnightAnimation(Texture2D spritesheet)
        {
            texture = spritesheet;
            timer = 0;
            currentFrame.X = 0;
            currentFrame.Y = (int)KnightMovementStates.Idle;
            prevMovementState = KnightMovementStates.Idle;
        }

        public void Update(float deltaTime, Vector2 velocity, KnightMovementStates knightMovementState, KnightMovementDirection knightMovementDirection)
        {
            if (knightMovementState != prevMovementState)
            {
                currentFrame.X = 0;
                currentFrame.Y = (int)knightMovementState;
                timer = 0;
            }

            float absVelocity = velocity.X > 0 ? velocity.X : -velocity.X;

            if (knightMovementState == KnightMovementStates.Run)
            {
                if (timer >= 1 / (absVelocity * 6))
                {
                    currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 0 : currentFrame.X + 1;
                    timer = 0;
                }
            }
            else if (knightMovementState == KnightMovementStates.Idle)
            {
                if (timer >= animationDuration[(int)knightMovementState])
                {
                    currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 0 : currentFrame.X + 1;
                    timer = 0;
                }
            }
            else if (knightMovementState == KnightMovementStates.Dead)
            {
                if (currentFrame.X < 2 && timer >= animationDuration[(int)knightMovementState] * 1.25f ||
                  (currentFrame.X >= 2 && timer >= animationDuration[(int)knightMovementState]))
                {
                    currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 2 : currentFrame.X + 1;
                    timer = 0;
                }
            }
            timer += deltaTime;


            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * (frameSize[(int)knightMovementState][0] + 1)),
                                maxFrameHight * (int)currentFrame.Y + 1,
								frameSize[(int)knightMovementState][0],
                                frameSize[(int)knightMovementState][1]);
            if (knightMovementDirection == KnightMovementDirection.Left)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;

            prevMovementState = knightMovementState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Knight.invincibilityTimer > 0)
            {
                if ((int)(Knight.invincibilityTimer / 0.1f) % 2 == 1)
                    spriteBatch.Draw(texture, position, sourceRect, new Color(0xEF3535FFu), 0, Vector2.Zero, GameManager.gameScale, spriteEffects, 0);
                else
                    spriteBatch.Draw(texture, position, sourceRect, new Color(0x8B0F0FFFu), 0, Vector2.Zero, GameManager.gameScale, spriteEffects, 0);
            }
            else
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, Vector2.Zero, GameManager.gameScale, spriteEffects, 0);
        }
    }
}
