using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using System.Reflection.Metadata;
using System.Reflection.Emit;
using NaamGaatNogKomen.Classes.Scripts.Enemies;

namespace NaamGaatNogKomen.Classes.Scripts
{
    internal class GameManager
    {
        private static Knight knight;
        private static MapGenerator mapGenerator;
        private static MonstersManager monstersManager;
        private static Texture2D plainTexture;
        private static int level;
        private static int lives;
        private static float movingLeftRemaining;


        public static readonly float gameScale = 3f;
        public static readonly int mapWidth = (int)(24 * 16 * gameScale);
        public static readonly int mapHeight = (int)(16 * 16 * gameScale);
        //public static readonly int level1FinshLineX = (int)(3263 * gameScale);
        //public static readonly int level2FinshLineX = (int)(1788 * gameScale);


        public GameManager(GraphicsDeviceManager graphics)
        {
            knight = new Knight();
            mapGenerator = new MapGenerator();
            monstersManager = new MonstersManager();
            plainTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            plainTexture.SetData(new Color[] { Color.White });
            level = 0;
            lives = 3;
            movingLeftRemaining = 0;
        }


        public void LoadContent(ContentManager content)
        {
            knight.LoadContent(content);
            mapGenerator.LoadContent(content);
            monstersManager.LoadContent(content);
            GoToNextLevel();
        }



        public void Update(float deltaTime)
        {
            knight.Update(deltaTime);
            mapGenerator.Update(deltaTime);
            monstersManager.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapGenerator.DrawBackground(spriteBatch);
            for (int i = 0; i < lives; ++i)
                DrawPixelHeart(spriteBatch, (int)(10 * gameScale) + (int)(12 * gameScale) * i, (int)(0.9f * MapGenerator.tileSize * gameScale), (int)gameScale, Color.DarkRed);
            monstersManager.Draw(spriteBatch);
            mapGenerator.DrawPlatform(spriteBatch);
            knight.Draw(spriteBatch);

        }
        public static void MoveMapLeft(float amount)
        {
            movingLeftRemaining += amount - (int)amount;
            amount += (int)movingLeftRemaining;
            movingLeftRemaining -= (int)movingLeftRemaining;

            mapGenerator.MoveLeft((int)amount);
            monstersManager.MoveLeft((int)amount);
        }
        public static int HitMap(Hitbox hitbox, bool withX, bool withY)
        {
            foreach (Hitbox collider in mapGenerator.colliders)
            {
                if (hitbox.rectangle.Intersects(collider.rectangle))
                {
                    int overlapX = Math.Max(0, Math.Min(hitbox.rectangle.Right, collider.rectangle.Right) -
                        Math.Max(hitbox.rectangle.Left, collider.rectangle.Left));

                    int overlapY = Math.Max(0, Math.Min(hitbox.rectangle.Bottom, collider.rectangle.Bottom) -
                        Math.Max(hitbox.rectangle.Top, collider.rectangle.Top));

                    if (withX && !withY && overlapX > 0 && overlapY > 0)
                        return overlapX;
                    else if (withY && !withX && overlapY > 0 && overlapY > 0)
                        return overlapY;
                    else if (withY && withX && overlapX > 0 && overlapY > 0)
                        return 1;
                }
            }
            return 0;
        }

        public static bool HitSpikes(Hitbox hitbox)
        {
            foreach (Hitbox collider in mapGenerator.spikes)
            {
                if (hitbox.rectangle.Intersects(collider.rectangle))
                {
                    --lives;

                    if (lives == 0)
                        knight.DeathRoutine();
                    return true;
                }
            }
            return false;
        }
        public static bool HitMonster(Hitbox hitbox, KnightMovementStates knightMovementStates, bool isInvincible)
        {
            if (!isInvincible)
            {
                foreach (var monster in monstersManager.Monster1List)
                {
                    if (hitbox.rectangle.Intersects(monster.hitbox.rectangle))
                    {
                        --lives;

                        if (lives == 0)
                            knight.DeathRoutine();

                        return true;
                    }
                }

                foreach (var monster in monstersManager.Monster3List)
                {
                    if (hitbox.rectangle.Intersects(monster.hitbox.rectangle) ||
                        hitbox.rectangle.Intersects(monster.projectile.hitbox.rectangle))
                    {
                        --lives;

                        if (lives == 0)
                            knight.DeathRoutine();

                        return true;
                    }
                }
            }
            foreach (var monster in monstersManager.Monster2List)
            {
                if (hitbox.rectangle.Intersects(monster.hitbox.rectangle) && monster.IsAlive())
                {
                    if (knightMovementStates == KnightMovementStates.Fall)
                    {
                        monster.Die();
                        knight.Bounce();
                    }
                    else if (!isInvincible)
                    {
                        --lives;

                        if (lives == 0)
                            knight.DeathRoutine();

                        return true;
                    }
                }
            }
            return false;
        }
        public static bool TouchedFinishLine(Hitbox hitbox)
        {
            if (hitbox.rectangle.Intersects(mapGenerator.finishLine.rectangle))
                return true;

            return false;
        }

        public static void GoToNextLevel()
        {
            mapGenerator.LoadLevel(++level);
            monstersManager.LoadLevel(level);
            knight.GoToInitialPosition(level);
        }

        public static void DrawPixelHeart(SpriteBatch spriteBatch, int x, int y, int size, Color color)
        {

            Rectangle[] heart = new Rectangle[]
            {
                new Rectangle(x + size * 1, y + size * 0, size * 2, size),
                new Rectangle(x + size * 6, y + size * 0, size * 2, size),

                new Rectangle(x + size * 0, y + size * 1, size * 4, size),
                new Rectangle(x + size * 5, y + size * 1, size * 4, size),

                new Rectangle(x + size * 0, y + size * 2, size * 9, size * 2),
                new Rectangle(x + size * 1, y + size * 4, size * 7, size * 1),
                new Rectangle(x + size * 2, y + size * 5, size * 5, size * 1),
                new Rectangle(x + size * 3, y + size * 6, size * 3, size * 1),
                new Rectangle(x + size * 4, y + size * 7, size * 1, size * 1),
            };

            foreach (Rectangle rect in heart)
                spriteBatch.Draw(plainTexture, rect, color);
        }
    }
}
