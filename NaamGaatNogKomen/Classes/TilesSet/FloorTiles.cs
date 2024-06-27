using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaamGaatNogKomen
{
    public class FloorTiles
    {
        private Texture2D _texture;
        private int _tileWidth;
        private int _tileHeight;
        private List<Rectangle> _tiles;
        private Rectangle _sourceRectangle;

        public int TileHeight => _tileHeight;

        public FloorTiles(Texture2D texture, int tileWidth, int tileHeight, Rectangle sourceRectangle)
        {
            _texture = texture;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _tiles = new List<Rectangle>();
            _sourceRectangle = sourceRectangle;
        }

        public void GenerateTiles(int screenWidth, int screenHeight)
        {
            _tiles.Clear();
            int tileY = screenHeight - _tileHeight; // Position tiles at the bottom of the screen
            for (int x = 0; x < screenWidth; x += _tileWidth)
            {
                _tiles.Add(new Rectangle(x, tileY, _tileWidth, _tileHeight));
            }
        }

        public List<Rectangle> GetTiles()
        {
            return _tiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                spriteBatch.Draw(_texture, tile, _sourceRectangle, Color.White);
            }
        }
    }
}
