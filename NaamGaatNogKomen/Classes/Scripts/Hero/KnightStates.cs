using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using NaamGaatNogKomen.Classes.Scripts.Managers;
using BlackKnight.Classes.Interfaces;

namespace BlackKnight.Classes.Scripts.Hero
{
    internal class IdleState : IKnightState
    {
        public void Enter(Knight knight, float deltaTime = 0)
        {
            knight.velocity = Vector2.Zero;
            knight.currentFrame.X = 0;
            knight.timer = 0;

            knight.moveLeft = false;
            knight.moveRight = false;
        }
        public void HandleInput(Knight knight, KeyboardState keyboardState)
        {
            if (knight.isDead)
            {
                knight.TransitionToState(new DeadState());
                return;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                knight.moveLeft = true;
                knight.knightMovementDirection = KnightMovementDirection.Left;
                knight.TransitionToState(new RunState());
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                knight.moveRight = true;
                knight.knightMovementDirection = KnightMovementDirection.Right;
                knight.TransitionToState(new RunState());
            }

            if (keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up)) // Jump
            {
                knight.velocity.Y = -3.25f * GameManager.gameScale;
                knight.TransitionToState(new JumpState());
            }

        }
        public void Update(Knight knight, float deltaTime)
        {
            if (Knight.invincibilityTimer > 0)
            {
                GameManager.HitMonster(knight.hitbox, Knight.invincibilityTimer > 0);
                Knight.invincibilityTimer -= deltaTime;
            }
            else
            {
                if (GameManager.HitSpikes(knight.hitbox) || GameManager.HitMonster(knight.hitbox, false))
                    Knight.invincibilityTimer = Knight.invincibilityTime;
            }

        }
        public void PlayAnimation(Knight knight, float deltaTime)
        {
            if (knight.timer >= 0.175f) // time interval between frames
            {
                knight.currentFrame.X = knight.currentFrame.X + 1 >= 5 ? 0 : knight.currentFrame.X + 1;
                knight.timer = 0;
            }

            knight.timer += deltaTime;

            knight.sourceRect = new Rectangle((int)(1 + knight.currentFrame.X * (21 + 1)), 1, 21, 22);

            if (knight.knightMovementDirection == KnightMovementDirection.Left)
                knight.spriteEffects = SpriteEffects.FlipHorizontally;
            else
                knight.spriteEffects = SpriteEffects.None;
        }
    }

    internal class RunState : IKnightState
    {

        public void Enter(Knight knight, float deltaTime)
        {
            knight.currentFrame.X = 0;
            knight.timer = 0;


        }
        public void HandleInput(Knight knight, KeyboardState keyboardState)
        {
            if (knight.isDead)
            {
                knight.TransitionToState(new DeadState());
                return;
            }

            knight.moveLeft = false;
            knight.moveRight = false;
            if (keyboardState.IsKeyDown(Keys.Left))
                knight.moveLeft = true;
            else if (keyboardState.IsKeyDown(Keys.Right))
                knight.moveRight = true;

            if (keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up)) // Jump
            {
                knight.velocity.Y = -3.25f * GameManager.gameScale;
                knight.TransitionToState(new JumpState());
            }
        }
        public void Update(Knight knight, float deltaTime)
        {
            if (knight.moveLeft)
            {
                knight.velocity.X = Knight.Lerp(knight.velocity.X, -Knight.maxVelocityX, 0.75f * deltaTime);
            }
            else if (knight.moveRight)
            {
                knight.velocity.X = Knight.Lerp(knight.velocity.X, Knight.maxVelocityX, 0.75f * deltaTime);
            }
            else if (knight.velocity.X != 0)
            {
                if (knight.velocity.X > 0)
                {
                    knight.velocity.X = Knight.Lerp(knight.velocity.X, -0.66f * Knight.maxVelocityX, 0.9f * deltaTime);
                    if (knight.velocity.X < 0) knight.velocity.X = 0;
                }
                else
                {
                    knight.velocity.X = Knight.Lerp(knight.velocity.X, 0.66f * Knight.maxVelocityX, 0.9f * deltaTime);
                    if (knight.velocity.X > 0) knight.velocity.X = 0;
                }
            }
            else
            {
                knight.TransitionToState(new IdleState());
            }

            // collision detection
            if (knight.velocity.X != 0)
            {
                knight.position.X += knight.velocity.X;
                knight.hitbox.Update(knight.position);
                int overlabX = GameManager.HitMap(knight.hitbox, true, false);

                if (overlabX > 0 && overlabX < knight.knightWidth / 2)
                {
                    knight.spriteEffects = SpriteEffects.FlipVertically;

                    knight.position.X -= knight.velocity.X;
                    knight.velocity.X = 0;
                    knight.hitbox.Update(knight.position);

                    knight.TransitionToState(new IdleState());
                }
            }

            // stop at the left most of the screen
            if (knight.position.X < GameManager.scrollAmount)
            {
                knight.position.X = GameManager.scrollAmount;
                knight.velocity.X = 0;
                knight.hitbox.Update(knight.position);
            }

            if (knight.knightMovementDirection == KnightMovementDirection.Right &&
                knight.position.X - knight.scrollAmount + knight.knightWidth / 2 > GameManager.mapWidth / 2 &&
                knight.scrollAmount + GameManager.mapWidth < 44 * MapGenerator.tileSize * GameManager.gameScale)
            {
                knight.scrollAmount = knight.position.X + knight.knightWidth / 2 - GameManager.mapWidth / 2;
                GameManager.MoveMapLeft(knight.scrollAmount);
            }

            knight.position.Y += 1;
            knight.hitbox.Update(knight.position);
            int overlapY = GameManager.HitMap(knight.hitbox, false, true);
            if (overlapY == 0)
            {
                knight.TransitionToState(new FallState());
            }
            else
            {
                knight.position.Y -= 1;
                knight.velocity.Y = 0;
                knight.hitbox.Update(knight.position);
            }

            if (Knight.invincibilityTimer > 0)
            {
                GameManager.HitMonster(knight.hitbox, Knight.invincibilityTimer > 0);
                Knight.invincibilityTimer -= deltaTime;
            }
            else
            {
                if (GameManager.HitSpikes(knight.hitbox) || GameManager.HitMonster(knight.hitbox, false))
                    Knight.invincibilityTimer = Knight.invincibilityTime;
            }

            if (knight.velocity.X > 0)
                knight.knightMovementDirection = KnightMovementDirection.Right;
            else if (knight.velocity.X < 0)
                knight.knightMovementDirection = KnightMovementDirection.Left;
        }
        public void PlayAnimation(Knight knight, float deltaTime)
        {
            float absVelocity = knight.velocity.X > 0 ? knight.velocity.X : -knight.velocity.X;

            if (knight.timer >= 1 / (absVelocity * 6)) // time interval between frames
            {
                knight.currentFrame.X = knight.currentFrame.X + 1 >= 8 ? 0 : knight.currentFrame.X + 1;
                knight.timer = 0;
            }

            knight.timer += deltaTime;

            knight.sourceRect = new Rectangle(
                                (int)(1 + knight.currentFrame.X * (21 + 1)),
                                Knight.maxFrameHight + 1,
                                21, 22);

            if (knight.knightMovementDirection == KnightMovementDirection.Left)
                knight.spriteEffects = SpriteEffects.FlipHorizontally;
            else
                knight.spriteEffects = SpriteEffects.None;
        }

    }

    internal class JumpState : IKnightState
    {
        public void Enter(Knight knight, float deltaTime = 0)
        {
            knight.sourceRect = new Rectangle(
                                1, Knight.maxFrameHight * 2 + 1,
                                21, 22);
        }
        public void HandleInput(Knight knight, KeyboardState keyboardState)
        {
            knight.moveLeft = false;
            knight.moveRight = false;
            if (keyboardState.IsKeyDown(Keys.Left))
                knight.moveLeft = true;
            else if (keyboardState.IsKeyDown(Keys.Right))
                knight.moveRight = true;
        }
        public void Update(Knight knight, float deltaTime)
        {
            if (!knight.isDead)
            {
                if (knight.moveLeft)
                {
                    knight.velocity.X = Knight.Lerp(knight.velocity.X, -Knight.maxVelocityX, 0.75f * deltaTime);
                }
                else if (knight.moveRight)
                {
                    knight.velocity.X = Knight.Lerp(knight.velocity.X, Knight.maxVelocityX, 0.75f * deltaTime);
                }
                else if (knight.velocity.X != 0)
                {
                    if (knight.velocity.X > 0)
                    {
                        knight.velocity.X = Knight.Lerp(knight.velocity.X, -0.66f * Knight.maxVelocityX, 0.9f * deltaTime);
                        if (knight.velocity.X < 0) knight.velocity.X = 0;
                    }
                    else
                    {
                        knight.velocity.X = Knight.Lerp(knight.velocity.X, 0.66f * Knight.maxVelocityX, 0.9f * deltaTime);
                        if (knight.velocity.X > 0) knight.velocity.X = 0;
                    }
                }

                // collision detection
                if (knight.velocity.X != 0)
                {
                    knight.position.X += knight.velocity.X;
                    knight.hitbox.Update(knight.position);
                    int overlabX = GameManager.HitMap(knight.hitbox, true, false);

                    if (overlabX > 0 && overlabX < knight.knightWidth / 2)
                    {
                        knight.position.X -= knight.velocity.X;
                        knight.velocity.X = 0;
                        knight.hitbox.Update(knight.position);
                    }
                }

                // stop at the left most of the screen
                if (knight.position.X < GameManager.scrollAmount)
                {
                    knight.position.X = GameManager.scrollAmount;
                    knight.velocity.X = 0;
                    knight.hitbox.Update(knight.position);
                }

                if (knight.knightMovementDirection == KnightMovementDirection.Right &&
                    knight.position.X - knight.scrollAmount + knight.knightWidth / 2 > GameManager.mapWidth / 2 &&
                    knight.scrollAmount + GameManager.mapWidth < 44 * MapGenerator.tileSize * GameManager.gameScale)
                {
                    knight.scrollAmount = knight.position.X + knight.knightWidth / 2 - GameManager.mapWidth / 2;
                    GameManager.MoveMapLeft(knight.scrollAmount);
                }

                knight.velocity.Y += Knight.gravity * deltaTime;
                knight.position.Y += knight.velocity.Y;
                knight.hitbox.Update(knight.position);

                if (knight.velocity.Y >= 0)
                    knight.TransitionToState(new FallState());

                if (Knight.invincibilityTimer > 0)
                {
                    GameManager.HitMonster(knight.hitbox, Knight.invincibilityTimer > 0);
                    Knight.invincibilityTimer -= deltaTime;
                }
                else
                {
                    if (GameManager.HitSpikes(knight.hitbox) || GameManager.HitMonster(knight.hitbox, false))
                        Knight.invincibilityTimer = Knight.invincibilityTime;
                }

                if (knight.velocity.X > 0)
                    knight.knightMovementDirection = KnightMovementDirection.Right;
                else if (knight.velocity.X < 0)
                    knight.knightMovementDirection = KnightMovementDirection.Left;
            }
            else
            {
                knight.velocity.Y += Knight.gravity * deltaTime;
                knight.position.Y += knight.velocity.Y;
                knight.hitbox.Update(knight.position);

                if (knight.velocity.Y >= 0)
                    knight.TransitionToState(new FallState());
            }
        }
        public void PlayAnimation(Knight knight, float deltaTime)
        {
            if (knight.knightMovementDirection == KnightMovementDirection.Left)
                knight.spriteEffects = SpriteEffects.FlipHorizontally;
            else
                knight.spriteEffects = SpriteEffects.None;
        }
    }

    internal class FallState : IKnightState
    {
        public void Enter(Knight knight, float deltaTime = 0)
        {
            knight.sourceRect = new Rectangle(
                                1, Knight.maxFrameHight * 3 + 1,
                                21, 22);
        }
        public void HandleInput(Knight knight, KeyboardState keyboardState)
        {
            knight.moveLeft = false;
            knight.moveRight = false;
            if (keyboardState.IsKeyDown(Keys.Left))
                knight.moveLeft = true;
            else if (keyboardState.IsKeyDown(Keys.Right))
                knight.moveRight = true;
        }
        public void Update(Knight knight, float deltaTime)
        {
            if (!knight.isDead)
            {
                if (knight.moveLeft)
                {
                    knight.velocity.X = Knight.Lerp(knight.velocity.X, -Knight.maxVelocityX, 0.75f * deltaTime);
                }
                else if (knight.moveRight)
                {
                    knight.velocity.X = Knight.Lerp(knight.velocity.X, Knight.maxVelocityX, 0.75f * deltaTime);
                }
                else if (knight.velocity.X != 0)
                {
                    if (knight.velocity.X > 0)
                    {
                        knight.velocity.X = Knight.Lerp(knight.velocity.X, -0.66f * Knight.maxVelocityX, 0.9f * deltaTime);
                        if (knight.velocity.X < 0) knight.velocity.X = 0;
                    }
                    else
                    {
                        knight.velocity.X = Knight.Lerp(knight.velocity.X, 0.66f * Knight.maxVelocityX, 0.9f * deltaTime);
                        if (knight.velocity.X > 0) knight.velocity.X = 0;
                    }
                }

                // collision detection
                if (knight.velocity.X != 0)
                {
                    knight.position.X += knight.velocity.X;
                    knight.hitbox.Update(knight.position);
                    int overlabX = GameManager.HitMap(knight.hitbox, true, false);

                    if (overlabX > 0 && overlabX < knight.knightWidth / 2)
                    {
                        knight.position.X -= knight.velocity.X;
                        knight.velocity.X = 0;
                        knight.hitbox.Update(knight.position);
                    }
                }

                // stop at the left most of the screen
                if (knight.position.X < GameManager.scrollAmount)
                {
                    knight.position.X = GameManager.scrollAmount;
                    knight.velocity.X = 0;
                    knight.hitbox.Update(knight.position);
                }

                if (knight.knightMovementDirection == KnightMovementDirection.Right &&
                    knight.position.X - knight.scrollAmount + knight.knightWidth / 2 > GameManager.mapWidth / 2 &&
                    knight.scrollAmount + GameManager.mapWidth < 44 * MapGenerator.tileSize * GameManager.gameScale)
                {
                    knight.scrollAmount = knight.position.X + knight.knightWidth / 2 - GameManager.mapWidth / 2;
                    GameManager.MoveMapLeft(knight.scrollAmount);
                }

                knight.velocity.Y += Knight.gravity * deltaTime;
                knight.position.Y += knight.velocity.Y;
                knight.hitbox.Update(knight.position);

                int overlapY = GameManager.HitMap(knight.hitbox, false, true);
                if (overlapY > 0 && overlapY <= knight.knightHeight * GameManager.gameScale / 2)
                {
                    if (knight.velocity.Y > 0)
                        knight.position.Y -= overlapY;
                    else
                        knight.position.Y += overlapY;

                    knight.hitbox.Update(knight.position);
                    if (knight.velocity.Y > 0) // was falling and hit the ground
                    {
                        knight.velocity.Y = 0;

                        if (knight.velocity.X != 0)
                            knight.TransitionToState(new RunState());
                        else
                            knight.TransitionToState(new IdleState());
                    }

                }

                if (Knight.invincibilityTimer > 0)
                {
                    GameManager.HitMonster(knight.hitbox, Knight.invincibilityTimer > 0, true);
                    Knight.invincibilityTimer -= deltaTime;
                }
                else
                {
                    if (GameManager.HitSpikes(knight.hitbox) || GameManager.HitMonster(knight.hitbox, false, true))
                        Knight.invincibilityTimer = Knight.invincibilityTime;
                }

                if (knight.velocity.X > 0)
                    knight.knightMovementDirection = KnightMovementDirection.Right;
                else if (knight.velocity.X < 0)
                    knight.knightMovementDirection = KnightMovementDirection.Left;
            }
            else
            {
                knight.velocity.Y += Knight.gravity * deltaTime;
                knight.position.Y += knight.velocity.Y;
                knight.hitbox.Update(knight.position);

                int overlapY = GameManager.HitMap(knight.hitbox, false, true);

                if (overlapY > 0 && overlapY <= knight.knightHeight * GameManager.gameScale / 2)
                {
                    if (knight.velocity.Y > 0)
                        knight.position.Y -= overlapY;
                    else
                        knight.position.Y += overlapY;

                    knight.hitbox.Update(knight.position);
                    if (knight.velocity.Y > 0) // was falling and hit the ground
                    {
                        knight.velocity = Vector2.Zero;
                        knight.TransitionToState(new DeadState());
                    }
                }
            }
        }
        public void PlayAnimation(Knight knight, float deltaTime)
        {
            if (knight.knightMovementDirection == KnightMovementDirection.Left)
                knight.spriteEffects = SpriteEffects.FlipHorizontally;
            else
                knight.spriteEffects = SpriteEffects.None;
        }
    }

    internal class DeadState : IKnightState
    {
        public void Enter(Knight knight, float deltaTime = 0)
        {
            if (knight.knightMovementDirection == KnightMovementDirection.Left)
                knight.spriteEffects = SpriteEffects.FlipHorizontally;
            else
                knight.spriteEffects = SpriteEffects.None;

            knight.currentFrame.X = 0;
            knight.timer = 0;

            knight.position.Y -= 5 * GameManager.gameScale;
            knight.position.X -= 13 * GameManager.gameScale;

            Knight.invincibilityTimer = 0;
        }
        public void HandleInput(Knight knight, KeyboardState keyboardState) { }
        public void Update(Knight knight, float deltaTime) { }
        public void PlayAnimation(Knight knight, float deltaTime)
        {
            if (knight.currentFrame.X < 2 && knight.timer >= 0.4f * 1.25f ||
                                    (knight.currentFrame.X >= 2 && knight.timer >= 0.4f))
            {
                knight.currentFrame.X = knight.currentFrame.X + 1 >= 7 ? 2 : knight.currentFrame.X + 1;
                knight.timer = 0;
            }

            knight.timer += deltaTime;

            knight.sourceRect = new Rectangle(
                                (int)(1 + knight.currentFrame.X * (44 + 1)),
                                Knight.maxFrameHight * 4 + 1,
                                44, 27);
        }
    }
}
