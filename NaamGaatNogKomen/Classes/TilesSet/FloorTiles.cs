using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.TilesSet
{
    internal class FloorTiles
    {
        private Texture2D floors; 
        private Animation flooranimation = new Animation();
        private Vector2 position = new Vector2(100, 100);
        public FloorTiles(Texture2D floor)
        {
            this.floors = floor;
            flooranimation.GetFramesFromTexture(floor.Width, floor.Height,4,1);
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Animation currentAnimation = flooranimation;
            spriteBatch.Draw(floors, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White);
        }
    }
}
