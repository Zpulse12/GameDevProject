using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.TilesSet
{
    public class FloorTiles
    {
        private Texture2D floors;
        private Animation floorAnimation = new Animation();

        public FloorTiles(Texture2D floor)
        {
            this.floors = floor;
            floorAnimation.GetFramesFromTexture(floor.Width, floor.Height, 4, 1);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            AnimationFrame currentFrame = floorAnimation.CurrentFrame;
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, GraphicsDeviceManager.DefaultBackBufferWidth, floors.Height);
            spriteBatch.Draw(floors, destinationRectangle, currentFrame.SourceRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            floorAnimation.Update(gameTime);
        }
    }



}
