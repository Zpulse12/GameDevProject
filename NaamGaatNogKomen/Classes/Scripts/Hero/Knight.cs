using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using System.Collections.Generic;

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
        public Hitbox hitbox;
        private bool isJumping;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private KnightAnimation animation;
        private KnightMovementStates knightMovementStates;
        private KnightMovementStates previousMovementStates;
        private KnightMovementDirection knightMovementDirection;

        private int knightWidth;
        private int knightHeight;
        private readonly float gravity = 8f * GameManager.gameScale;

        private readonly Dictionary<KnightMovementStates, Vector2> knightSize = new Dictionary<KnightMovementStates, Vector2>
        {
            { KnightMovementStates.Idle, new Vector2(17, 22) },
            { KnightMovementStates.Run, new Vector2(21, 22) },
            { KnightMovementStates.Jump, new Vector2(17, 22) },
            { KnightMovementStates.Fall, new Vector2(19, 22) }
        };
        public static readonly float maxVelocityX = 1f * GameManager.gameScale;


        public Knight()
        {
            isJumping = false;
            velocity.X = 0;
            velocity.Y = 0;

            position.X = 50;
            position.Y = 150;

            knightMovementStates = KnightMovementStates.Idle;
            knightMovementDirection = KnightMovementDirection.Right;
            knightWidth = (int)knightSize[knightMovementStates].X;
            knightHeight = (int)knightSize[knightMovementStates].Y;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("newknight");
            hitbox = new Hitbox(new Rectangle((int)(2 * GameManager.gameScale), 0, (int)(17 * GameManager.gameScale), (int)(22 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);

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

    
            if (position.X < 0)
            {
                position.X = 0;
                velocity.X = 0;
                hitbox.Update(position);

            }
            if (velocity.X != 0)
            {
                
                position.X += velocity.X;
                hitbox.Update(position);
                int overlabX = GameManager.HitMap(hitbox, true, false);

                if (overlabX > 0 && overlabX < (knightWidth) / 2)
                {
                    position.X -= velocity.X;
                    velocity.X = 0;

                    hitbox.Update(position);
                }
            }

            // move the map when the knight is in the middle of the screen
            if (position.X + 10 * GameManager.gameScale / 2 > GameManager.mapWidth / 2)
            {
                GameManager.MoveMapLeft((position.X + knightWidth / 2) - GameManager.mapWidth / 2);
               position.X = GameManager.mapWidth / 2 - knightWidth / 2;
                hitbox.Update(position);

            }

            if ((keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up)) && !isJumping) // Jump
            {
                isJumping = true;
                velocity.Y = -3.25f * GameManager.gameScale;
                knightMovementStates = KnightMovementStates.Jump;
            }

            if (isJumping)
            {
                velocity.Y += gravity * deltaTime;
                position.Y += velocity.Y;
                hitbox.Update(position);

                int overlapY = GameManager.HitMap(hitbox, false, true);
                if (overlapY > 0 && overlapY <= (knightHeight * GameManager.gameScale) / 2)
                {
                    if (velocity.Y > 0)
                        position.Y -= overlapY;
                    else
                        position.Y += overlapY;

                    hitbox.Update(position);
                    if (velocity.Y > 0) // was falling and hit the ground
                    {
                        velocity.Y = 0;
                        isJumping = false;
                    }
                }
            }

            else 
            {

                position.Y += 1;
                hitbox.Update(position);
                int overlapY = GameManager.HitMap(hitbox, false, true);
                if (overlapY == 0)
                {
                    isJumping = true;
                    knightMovementStates = KnightMovementStates.Fall;
                }
                else
                {
                    position.Y -= 1;
                    velocity.Y = 0;
                    hitbox.Update(position);
                }
            }
                
            if (velocity.X == 0 && velocity.Y == 0)
                knightMovementStates = KnightMovementStates.Idle;
            else if (velocity.Y < 0)
                knightMovementStates = KnightMovementStates.Jump;
            else if (velocity.Y > 0)
                knightMovementStates = KnightMovementStates.Fall;
            else
                knightMovementStates = KnightMovementStates.Run;

            if (velocity.X > 0)
                knightMovementDirection = KnightMovementDirection.Right;
            else if (velocity.X < 0)
                knightMovementDirection = KnightMovementDirection.Left;

            // Check if the movement state has changed 
            // update position and size accordingly
            //if (previousMovementStates != knightMovementStates)
            //{
            //	knightWidth = (int)(knightSize[knightMovementStates].X * GameManager.gameScale);
            //	knightHeight = (int)(knightSize[knightMovementStates].Y * GameManager.gameScale)
            //	int widthDiff = (int)(knightSize[previousMovementStates].X - knightSize[knightMovementStates].X);
            //	widthDiff *= (int)GameManager.gameScale;
            //	position.X += widthDiff;
            //	hitbox = new Hitbox(new Rectangle(0, 0, knightWidth, knightHeight), Vector2.Zero);
            //	hitbox.Update(position);
            //	previousMovementStates = knightMovementStates;
            //}
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