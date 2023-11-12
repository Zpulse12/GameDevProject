using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.TilesSet
{
    internal class Tiles
    {
        protected Texture2D texture;
        protected Vector2 position; // gaat de posititie bepalen van de texture

        public Tiles(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
        public virtual void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, position, Color.White);//Color.White lets the sprite be drawen with the original texture
        }
    }
}
