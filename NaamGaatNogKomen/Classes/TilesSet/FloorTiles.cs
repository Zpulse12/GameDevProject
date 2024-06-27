using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

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

        public void GenerateTiles(int screenWidth, int screenHeight)
        {
            tiles.Clear();
            int tileY = screenHeight - tileHeight; // Position tiles at the bottom of the screen
            for (int x = 0; x < screenWidth; x += tileWidth)
            {
                // Create a gap at a specific position
                if (x > screenWidth / 2 && x < screenWidth / 2 + tileWidth * 2)
                {
                    continue; // Skip adding tiles to create a gap
                }

                tiles.Add(new Rectangle(x, tileY, tileWidth, tileHeight));
                Debug.WriteLine($"Tile Position: {x}, {tileY}"); // Print each tile's position
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
