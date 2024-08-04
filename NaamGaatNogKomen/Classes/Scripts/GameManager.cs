using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using System.Reflection.Metadata;

namespace NaamGaatNogKomen.Classes.Scripts
{
    internal class GameManager
    {
        private static Knight knight;
        private static MapGenerator mapGenerator;
        private static int lives;
        private static float movingLeftRemaining;


        public static readonly float gameScale = 3f;
        public static readonly int mapWidth = (int)(24 * 16 * gameScale);
        public static readonly int mapHeight = (int)(16 * 16 * gameScale);
        //public static readonly int level1FinshLineX = (int)(3263 * gameScale);
        //public static readonly int level2FinshLineX = (int)(1788 * gameScale);


        public GameManager()
        {
            knight = new Knight();
            mapGenerator = new MapGenerator();
            lives = 3;
            movingLeftRemaining = 0;

        }


        public void LoadContent(ContentManager content)
        {
            knight.LoadContent(content);
            mapGenerator.LoadContent(content);
        }



        public void Update(float deltaTime)
        {
            knight.Update(deltaTime);
            mapGenerator.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mapGenerator.Draw(spriteBatch);
            knight.Draw(spriteBatch);
        }
        public static void MoveMapLeft(float amount)
        {
            movingLeftRemaining += amount - (int)amount;
            amount += (int)movingLeftRemaining;
            movingLeftRemaining -= (int)movingLeftRemaining;

            mapGenerator.MoveLeft((int)amount);
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
                if (hitbox.rectangle.Intersects(collider.rectangle))
                {
                    --lives;

                    if (lives == 0)
                    {
                        knight.DeathRoutine();
                    }

                    return true;
                }
            return false;
        }
    }
}
