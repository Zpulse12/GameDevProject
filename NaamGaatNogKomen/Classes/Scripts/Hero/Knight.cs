using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Scripts.Hero;

namespace NaamGaatNogKomen.Classes.Scripts.Hero
{
    enum KnightMovementStates
    {
        Idle,
        Run,
        Jump,
        Fall,
        Attack,
        Taking_Damage,
        Dead,
        Shield_Blocking
    }

    enum KnightMovementDirection
    {
        Right,
        Left
    }

    internal class Knight
    {
        private float scale;
        private bool isJumping;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private KnightAnimation animation;
        private KnightMovementStates knightMovementStates;
        private KnightMovementDirection knightMovementDirection;

        public static readonly float maxVelocityX = 0.75f * GameManager.gameScale;


        public Knight()
        {
            velocity.X = 0;
            velocity.Y = 0;

            position.X = 20;
            position.Y = 150;

            knightMovementStates = KnightMovementStates.Idle;
            knightMovementDirection = KnightMovementDirection.Right;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("newknight");


            animation = new KnightAnimation(texture);
        }

        public void Update(float deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                knightMovementStates = KnightMovementStates.Run;

                velocity.X = Lerp(velocity.X, -maxVelocityX, 0.75f * deltaTime);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                knightMovementStates = KnightMovementStates.Run;

                velocity.X = Lerp(velocity.X, maxVelocityX, 0.75f * deltaTime);
            }
            else if (velocity.X != 0)
            {
                if (velocity.X > 0)
                {
                    velocity.X = Lerp(velocity.X, -0.66f * maxVelocityX, 0.9f * deltaTime);
                    if (velocity.X < 0) velocity.X = 0;
                }
                else
                {
                    velocity.X = Lerp(velocity.X, 0.66f * maxVelocityX, 0.9f * deltaTime);
                    if (velocity.X > 0) velocity.X = 0;
                }
            }

            if (velocity.X != 0)
            {
                position.X += velocity.X;
            }
            else
            {
                knightMovementStates = KnightMovementStates.Idle;
            }

            if (velocity.X > 0)
            {
                knightMovementDirection = KnightMovementDirection.Right;
            }
            else if (velocity.X < 0)
            {
                knightMovementDirection = KnightMovementDirection.Left;
            }



            animation.position = position;
            animation.Update(deltaTime, velocity, knightMovementStates, knightMovementDirection);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

        private static float Lerp(float start, float end, float t)
        {
            return start * (1 - t) + end * t;
        }
    }
}