using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaamGaatNogKomen.Classes.TilesSet
{
    public class FloorTiles
    {
        private Texture2D floors;
        private int numberOfColumns; // Number of tiles in the width of the tileset
        private int tileWidth; // Width of each tile
        private int tileHeight; // Height of each tile
        private List<Rectangle> tiles;

        public FloorTiles(Texture2D floor, int numberOfColumns)
        {
            this.floors = floor;
            this.numberOfColumns = numberOfColumns;
            this.tileWidth = floors.Width / numberOfColumns;
            this.tileHeight = floors.Height / numberOfColumns;
            this.tiles = new List<Rectangle>();
        }

        public void GenerateTiles(int screenWidth)
        {
            tiles.Clear();
            // Generate bottom row
            for (int x = 0; x < screenWidth; x += tileWidth)
            {
                tiles.Add(new Rectangle(x, 600 - tileHeight, tileWidth, tileHeight));
            }
        }

        public List<Rectangle> GetWalkableTiles()
        {
            return tiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, (numberOfColumns - 1) * tileHeight, tileWidth, tileHeight);
            foreach (var tile in tiles)
            {
                spriteBatch.Draw(floors, tile, sourceRectangle, Color.White);
            }
        }

        public int TileHeight => tileHeight;

        public List<Rectangle> GetTiles()
        {
            return tiles;
        }
    }
}
