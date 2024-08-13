using BlackKnight.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Interfaces;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using NaamGaatNogKomen.Classes.Scripts.Managers;
using BlackKnight.Classes.Scripts.Hero;
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

    internal class Knight
    {
        private Texture2D texture;
        private IKnightState currentState;

        public bool isDead;
        public Hitbox hitbox;
        public bool moveLeft;
        public bool moveRight;
        public bool isFalling;
        public int knightWidth;
        public int knightHeight;
        public Vector2 position;
        public Vector2 velocity;
        public float scrollAmount;
        public KnightMovementDirection knightMovementDirection;

        public static float invincibilityTimer;
        public static float DeathAnimationTimer;
        public static readonly float invincibilityTime = 2;
        public static readonly Vector2 knightSize = new Vector2(18, 22); // w, h
        public static readonly float maxVelocityX = 1 * GameManager.gameScale;
        public static readonly float gravity = 8f * GameManager.gameScale;

        private readonly Dictionary<int, Vector2> intialPosition = new Dictionary<int, Vector2>
        {
            { 1, new Vector2(3 * MapGenerator.tileSize, (16 - 7) * MapGenerator.tileSize - 22) * GameManager.gameScale },
            { 2, new Vector2(1 * MapGenerator.tileSize, (16 - 8) * MapGenerator.tileSize - 22) * GameManager.gameScale }
        }; // w, h

        // animation related members

        public float timer;
        public Vector2 currentFrame; //this is the current frame the animation is using from the sprite sheet
        public Rectangle sourceRect; //this is used to select a portion of the spritesheet
        public SpriteEffects spriteEffects; // this can flip the animation horizontally or vertically

        public static readonly int maxFrameHight = 23;

        public Knight()
        {
            isDead = false;
            isFalling = false;
            scrollAmount = 0;
            invincibilityTimer = 0;
            DeathAnimationTimer = 4;

            velocity = Vector2.Zero;
            moveLeft = false;
            moveRight = false;

            currentState = new IdleState();
            knightMovementDirection = KnightMovementDirection.Right;

            knightWidth = (int)((knightSize.X + 1) * GameManager.gameScale);
            knightHeight = (int)((knightSize.Y + 1) * GameManager.gameScale);

            timer = 0;
            currentFrame = Vector2.Zero;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Knight");

            // collider for the knight
            hitbox = new Hitbox(new Rectangle((int)(2 * GameManager.gameScale), 0, (int)(17 * GameManager.gameScale), (int)(22 * GameManager.gameScale)), Vector2.Zero);
            hitbox.Update(position);
        }

        public void Update(float deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            currentState.HandleInput(this, keyboardState);
            currentState.Update(this, deltaTime);
            currentState.PlayAnimation(this, deltaTime);
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

        public static float Lerp(float start, float end, float t)
        {
            return start * (1 - t) + end * t;
        }

        public void DeathRoutine()
        {
            isDead = true;
        }

        public void Bounce()
        {
            velocity.Y = -2.5f * GameManager.gameScale;
            TransitionToState(new JumpState());
        }

        public void GoToInitialPosition(int level)
        {
            isDead = false;
            invincibilityTimer = 0;
            DeathAnimationTimer = 4;

            velocity.X = 0;
            velocity.Y = 0;

            TransitionToState(new IdleState());
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

        public void TransitionToState(IKnightState newState)
        {
            currentState = newState;
            currentState.Enter(this);
        }
    }
}