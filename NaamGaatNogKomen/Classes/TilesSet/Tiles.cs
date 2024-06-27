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
        protected Vector2 position;

        public Tiles(Texture2D texture, float screenWidth, float screenHeight)
        {
            this.texture = texture;
            // Start de positie vanaf de onderkant van het scherm
            this.position = new Vector2(0, screenHeight - texture.Height);
        }

        public virtual void Draw(SpriteBatch sprite, Rectangle sourceRectangle)
        {
            sprite.Draw(texture, position, sourceRectangle, Color.White);
        }
    }

}
