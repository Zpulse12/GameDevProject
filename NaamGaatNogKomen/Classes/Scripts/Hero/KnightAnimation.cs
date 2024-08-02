using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NaamGaatNogKomen.Classes.Scripts.Hero
{
    internal class KnightAnimation
    {
        public Vector2 position;
        public float animationDuration; 
        public SpriteEffects spriteEffects; 

        private float timer;
        private Texture2D texture; 
        private Vector2 currentFrame; 
        private Rectangle sourceRect; 
        private KnightMovementStates prevMovementState;

        private readonly int[][] frameSize = new int[][] { new int[] { 17, 22 }, new int[] { 21, 23 } };
        private readonly int[] framesCount = new int[] { 5, 8 };
        private readonly int maxFrameHight = 23;


        public KnightAnimation(Texture2D spritesheet)
        {
            timer = 0;
            currentFrame.X = 0;
            currentFrame.Y = (int)KnightMovementStates.Idle;
            texture = spritesheet;
            animationDuration = 0.15f;

            spriteEffects = SpriteEffects.None;
            prevMovementState = KnightMovementStates.Idle;
        }


        public void Update(float deltaTime, KnightMovementStates knightMovementState, KnightMovementDirection knightMovementDirection)
        {
            if (knightMovementState != prevMovementState) 
            {
                currentFrame.X = 0;
                currentFrame.Y = (int)knightMovementState;
            }


            if (timer >= animationDuration)
            {
                currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 0 : currentFrame.X + 1;
                timer = 0;
            }
            timer += deltaTime;

            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * (frameSize[(int)knightMovementState][0] + 1)),
                                maxFrameHight * (int)currentFrame.Y,
                                frameSize[(int)knightMovementState][0],
                                frameSize[(int)knightMovementState][1]);

            spriteEffects = SpriteEffects.None;
            if (knightMovementDirection == KnightMovementDirection.Left)
                spriteEffects = SpriteEffects.FlipHorizontally;

            prevMovementState = knightMovementState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, Vector2.Zero, 4, spriteEffects, 0);
        }
    }
}
