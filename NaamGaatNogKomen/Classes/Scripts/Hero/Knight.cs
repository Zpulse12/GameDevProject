using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Interfaces;
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
        Dead
    }
    enum KnightMovementDirection
    {
        Right,
        Left
    }
    internal class Knight : IAnimatable
    {
        public Hitbox hitbox;
        private bool isDead;
        private bool isJumping;
        private int knightWidth;
        private int knightHeight;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private float scrollAmount;
        private KnightMovementStates knightMovementState;
        private KnightMovementDirection knightMovementDirection;

        public static float invincibilityTimer;
        public static float DeathAnimationTimer;
        public static readonly Vector2 knightSize = new Vector2(18, 22); // w, h
        public static readonly float maxVelocityX = 1 * GameManager.gameScale;
        private readonly float gravity = 8f * GameManager.gameScale;
        private readonly float invincibilityTime = 2;
        private readonly Dictionary<int, Vector2> intialPosition = new Dictionary<int, Vector2>
        {
            { 1, new Vector2(3 * MapGenerator.tileSize, (16 - 7) * MapGenerator.tileSize - 22) * GameManager.gameScale },
            { 2, new Vector2(1 * MapGenerator.tileSize, (16 - 8) * MapGenerator.tileSize - 22) * GameManager.gameScale }
        }; // w, h

        private float timer;
        private Vector2 currentFrame;
        private Rectangle sourceRect;
        private SpriteEffects spriteEffects;
        private KnightMovementStates prevMovementState;
        private readonly int maxFrameHight = 23;
        private readonly int[] framesCount = new int[] { 5, 8, 1, 1, 7 };
        private readonly Dictionary<KnightMovementStates, Vector2> frameSize = new Dictionary<KnightMovementStates, Vector2>
        {
            { KnightMovementStates.Idle, new Vector2(21, 22)},
            { KnightMovementStates.Run, new Vector2(21, 22)},
            { KnightMovementStates.Jump, new Vector2(21, 22)},
            { KnightMovementStates.Fall, new Vector2(21, 22)},
            { KnightMovementStates.Dead, new Vector2(44, 27)}
        }; // w, h
        private readonly float[] animationDuration = new float[] { 0.175f, 0.075f, 1f, 1f, 0.4f }; //this is the full duration of the animation
        public Knight()
        {
            isDead = false;
            isJumping = false;
            scrollAmount = 0;
            invincibilityTimer = 0;
            DeathAnimationTimer = 4;
            velocity = Vector2.Zero;
            knightMovementState = KnightMovementStates.Idle;
            knightMovementDirection = KnightMovementDirection.Right;
            knightWidth = (int)((knightSize.X + 1) * GameManager.gameScale);
            knightHeight = (int)((knightSize.Y + 1) * GameManager.gameScale);
            timer = 0;
            currentFrame = Vector2.Zero;
            prevMovementState = knightMovementState;
        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Knight");
            hitbox = new Hitbox(new Rectangle((int)(2 * GameManager.gameScale), 0, (int)(17 * GameManager.gameScale), (int)(22 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);
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
                    if (overlabX > 0 && overlabX < knightWidth / 2)
                    {
                        position.X -= velocity.X;
                        velocity.X = 0;
                        hitbox.Update(position);
                    }
                }
                if (position.X < GameManager.scrollAmount)
                {
                    position.X = GameManager.scrollAmount;
                    velocity.X = 0;
                    hitbox.Update(position);
                }
                if (knightMovementState != KnightMovementStates.Idle &&
                    knightMovementDirection == KnightMovementDirection.Right &&
                    position.X - scrollAmount + knightWidth / 2 > GameManager.mapWidth / 2 &&
                    scrollAmount + GameManager.mapWidth < 44 * MapGenerator.tileSize * GameManager.gameScale)
                {
                    scrollAmount = position.X + knightWidth / 2 - GameManager.mapWidth / 2;
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
                    if (overlapY > 0 && overlapY <= knightHeight * GameManager.gameScale / 2)
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
                        knightMovementState = KnightMovementStates.Fall;
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
                    GameManager.HitMonster(hitbox, knightMovementState, invincibilityTimer > 0);
                    invincibilityTimer -= deltaTime;
                }
                else
                {
                    if (GameManager.HitSpikes(hitbox) || GameManager.HitMonster(hitbox, knightMovementState, false))
                    {
                        invincibilityTimer = invincibilityTime;
                    }
                }
                if (velocity.X == 0 && velocity.Y == 0)
                    knightMovementState = KnightMovementStates.Idle;
                else if (velocity.Y < 0)
                    knightMovementState = KnightMovementStates.Jump;
                else if (velocity.Y > 0)
                    knightMovementState = KnightMovementStates.Fall;
                else
                    knightMovementState = KnightMovementStates.Run;
                if (velocity.X > 0)
                    knightMovementDirection = KnightMovementDirection.Right;
                else if (velocity.X < 0)
                    knightMovementDirection = KnightMovementDirection.Left;
            }
            else
            {
                if (isJumping)
                {
                    velocity.Y += gravity * deltaTime;
                    position.Y += velocity.Y;
                    hitbox.Update(position);
                    int overlapY = GameManager.HitMap(hitbox, false, true);
                    if (overlapY > 0 && overlapY <= knightHeight * GameManager.gameScale / 2)
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
                else if (knightMovementState != KnightMovementStates.Dead)
                {
                    knightMovementState = KnightMovementStates.Dead;
                    position.Y -= 5 * GameManager.gameScale;
                    position.X -= 13 * GameManager.gameScale;
                    invincibilityTimer = 0;
                }
            }

            PlayAnimation(deltaTime);
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
            knightMovementState = KnightMovementStates.Jump;
        }
        public void GoToInitialPosition(int level)
        {
            isDead = false;
            isJumping = false;
            invincibilityTimer = 0;
            DeathAnimationTimer = 4;
            velocity.X = 0;
            velocity.Y = 0;
            knightMovementState = KnightMovementStates.Idle;
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
        public void PlayAnimation(float deltaTime)
        {
            if (knightMovementState != prevMovementState) // Movement State changed
            {
                currentFrame.X = 0;
                currentFrame.Y = (int)knightMovementState;
                timer = 0;
            }
            float absVelocity = velocity.X > 0 ? velocity.X : -velocity.X;
            switch (knightMovementState)
            {
                case KnightMovementStates.Idle:
                    if (timer >= animationDuration[(int)knightMovementState]) // time interval between frames
                    {
                        currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 0 : currentFrame.X + 1;
                        timer = 0;
                    }
                    break;
                case KnightMovementStates.Run:
                    if (timer >= 1 / (absVelocity * 6)) // time interval between frames
                    {
                        currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 0 : currentFrame.X + 1;
                        timer = 0;
                    }
                    break;
                case KnightMovementStates.Dead:
                    if (currentFrame.X < 2 && timer >= animationDuration[(int)knightMovementState] * 1.25f ||
                        (currentFrame.X >= 2 && timer >= animationDuration[(int)knightMovementState]))
                    {
                        currentFrame.X = currentFrame.X + 1 >= framesCount[(int)knightMovementState] ? 2 : currentFrame.X + 1;
                        timer = 0;
                    }
                    break;
            }
            timer += deltaTime;
            sourceRect = new Rectangle(
                                (int)(1 + currentFrame.X * ((int)frameSize[knightMovementState].X + 1)),
                                maxFrameHight * (int)currentFrame.Y + 1,
                                (int)frameSize[knightMovementState].X,
                                (int)frameSize[knightMovementState].Y);
            if (knightMovementDirection == KnightMovementDirection.Left)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;
            prevMovementState = knightMovementState;
        }
    }
}