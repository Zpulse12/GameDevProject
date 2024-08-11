using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using NaamGaatNogKomen.Classes.Scripts.Managers;
using System.Collections.Generic;

namespace NaamGaatNogKomen.Classes.Scripts.Hero
{
    enum KnightMovementStates
    {
        Idle,
        Run,
        Jump,
        Fall,
        Dead,
        Attack,
        Taking_Damage,
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
        private bool isDead;
        private bool isJumping;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private float scrollAmount;
        private KnightAnimation animation;
        private KnightMovementStates knightMovementStates;
        private KnightMovementDirection knightMovementDirection;

        public static float invincibilityTimer;
        public static float DeathAnimationTimer;

        private int knightWidth;
        private int knightHeight;
        private readonly float gravity = 8f * GameManager.gameScale;
        private readonly float invincibilityTime = 2;

        public static readonly Vector2 knightSize = new Vector2(18, 22);

        private readonly Dictionary<int, Vector2> intialPosition = new Dictionary<int, Vector2>
        {
            { 1, new Vector2(3 * MapGenerator.tileSize, (16 - 7) * MapGenerator.tileSize - 22) * GameManager.gameScale },
            { 2, new Vector2(1 * MapGenerator.tileSize, (16 - 8) * MapGenerator.tileSize - 22) * GameManager.gameScale }
        };

        public static readonly float maxVelocityX = 1f * GameManager.gameScale;


        public Knight()
        {
            isDead = false;
            isJumping = false;
            scrollAmount = 0;
            invincibilityTimer = 0;
            DeathAnimationTimer = 4;

            velocity.X = 0;
            velocity.Y = 0;

            position.X = 50;
            position.Y = 150;

            knightMovementStates = KnightMovementStates.Idle;
            knightMovementDirection = KnightMovementDirection.Right;
            knightWidth = (int)((knightSize.X + 1) * GameManager.gameScale);
            knightHeight = (int)((knightSize.Y + 1) * GameManager.gameScale);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("knight");
            hitbox = new Hitbox(new Rectangle((int)(2 * GameManager.gameScale), 0, (int)(17 * GameManager.gameScale), (int)(22 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);

            animation = new KnightAnimation(texture);
        }

        public void Update(float deltaTime)
        {
            if (!isDead)
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    velocity.X = Lerp(velocity.X, -maxVelocityX, 0.75f * deltaTime);
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
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
                    hitbox.Update(position);
                    int overlabX = GameManager.HitMap(hitbox, true, false);

                    if (overlabX > 0 && overlabX < (knightWidth) / 2)
                    {
                        position.X -= velocity.X;
                        velocity.X = 0;

                        hitbox.Update(position);
                    }
                }

                // stop at the left most of the screen
                if (position.X < GameManager.scrollAmount)
                {
                    position.X = GameManager.scrollAmount;
                    velocity.X = 0;
                    hitbox.Update(position);
                }

                if (knightMovementStates != KnightMovementStates.Idle &&
                    knightMovementDirection == KnightMovementDirection.Right &&
                    position.X - scrollAmount + knightWidth / 2 > GameManager.mapWidth / 2 &&
                    scrollAmount + GameManager.mapWidth < 44 * MapGenerator.tileSize * GameManager.gameScale)
                {
                    scrollAmount = (position.X + knightWidth / 2) - GameManager.mapWidth / 2;
                    GameManager.MoveMapLeft(scrollAmount);
                }

                if ((keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up)) && !isJumping)
                {
                    isJumping = true;
                    velocity.Y = -3.25f * GameManager.gameScale;
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
                        if (velocity.Y > 0)
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

                if (invincibilityTimer > 0)
                {
                    GameManager.HitMonster(hitbox, knightMovementStates, invincibilityTimer > 0);
                    invincibilityTimer -= deltaTime;
                }
                else
                {
                    if (GameManager.HitSpikes(hitbox) || GameManager.HitMonster(hitbox, knightMovementStates, false))
                    {
                        invincibilityTimer = invincibilityTime;
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

                animation.position = position;
                animation.Update(deltaTime, velocity, knightMovementStates, knightMovementDirection);
            }
            else
            {
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
                        if (velocity.Y > 0)
                        {
                            velocity.Y = 0;
                            isJumping = false;
                        }
                    }
                    else if (knightMovementStates != KnightMovementStates.Dead)
                    {
                        knightMovementStates = KnightMovementStates.Dead;

                        position.Y -= 5 * GameManager.gameScale;
                        position.X -= 13 * GameManager.gameScale;
                        invincibilityTimer = 0;
                    }
                }


                animation.position = position;
                animation.Update(deltaTime, velocity, knightMovementStates, knightMovementDirection);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

        private static float Lerp(float start, float end, float t)
        {
            return start * (1 - t) + end * t;
        }

        public void DeathRoutine()
        {
            isDead = true;
        }
        public void Bounce()
        {
            isJumping = true;
            velocity.Y = -2.5f * GameManager.gameScale;
            knightMovementStates = KnightMovementStates.Jump;
        }

        public void GoToInitialPosition(int level)
        {
            isDead = false;
            isJumping = false;
            invincibilityTimer = 0;
            DeathAnimationTimer = 4;

            velocity.X = 0;
            velocity.Y = 0;

            knightMovementStates = KnightMovementStates.Idle;
            knightMovementDirection = KnightMovementDirection.Right;

            knightWidth = (int)((knightSize.X + 1) * GameManager.gameScale);
            knightHeight = (int)((knightSize.Y + 1) * GameManager.gameScale);
            position = intialPosition[level];
            hitbox.Update(position);
            scrollAmount = 0;

        }
        public Vector2 GetPostion()
        {
            return position;
        }
    }
}