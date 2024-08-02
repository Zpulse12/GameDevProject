using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using NaamGaatNogKomen.Classes.Scripts.Hero;

namespace NaamGaatNogKomen.Classes.Scripts
{
    internal class GameManager
    {
        private static Knight knight;


        public GameManager()
        {

        }


        public void LoadContent(ContentManager content)
        {
            knight = new Knight();
            knight.LoadContent(content);
        }


        public void Update(float deltaTime)
        {
            knight.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            knight.Draw(spriteBatch);
        }
    }
}
