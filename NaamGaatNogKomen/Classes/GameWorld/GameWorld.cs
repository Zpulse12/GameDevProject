using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Input;
using NaamGaatNogKomen.Classes.TilesSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.GameWorld
{
    internal class GameWorld
    {
        private FloorTiles floorTiles;
        private Hero hero;
        public GameWorld(Texture2D floortexture, Texture2D heroWalkTexture, Texture2D heroIdleTexture, IInputReader inputReader)
        {
            floorTiles = new FloorTiles(floortexture);
            hero = new Hero(heroWalkTexture, heroIdleTexture, inputReader);

        }
    }
}
