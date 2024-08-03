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



        public static readonly float gameScale = 3f;
        public static readonly int mapWidth = (int)(24 * 16 * gameScale);
        public static readonly int mapHeight = (int)(16 * 16 * gameScale);
        //public static readonly int level1FinshLineX = (int)(3263 * gameScale);
        //public static readonly int level2FinshLineX = (int)(1788 * gameScale);


        public GameManager()
        {
        }


        public void LoadContent(ContentManager content)
        {
            knight = new Knight();
            knight.LoadContent(content);

            mapGenerator = new MapGenerator();
            mapGenerator.LoadContent(content);
        }



        public void Update(float deltaTime)
        {
            knight.Update(deltaTime);
            mapGenerator.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            knight.Draw(spriteBatch);
            mapGenerator.Draw(spriteBatch);
        }
    }
}
