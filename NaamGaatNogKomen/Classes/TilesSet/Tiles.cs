using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.TilesSet
{
    internal class Tiles
    {
        protected Texture2D texture;
        protected Vector2 position;

        public Tiles(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
    }
}
