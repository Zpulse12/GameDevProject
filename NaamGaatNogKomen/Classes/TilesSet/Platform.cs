using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaamGaatNogKomen
{
    public class Platform
    {
        private Texture2D _texture;
        private List<Rectangle> _platforms;
        private Rectangle _sourceRectangle;

        public Platform(Texture2D texture, Rectangle sourceRectangle)
        {
            _texture = texture;
            _platforms = new List<Rectangle>();
            _sourceRectangle = sourceRectangle;
        }

        public void GeneratePlatforms(int screenWidth, int screenHeight)
        {
            _platforms.Clear();
            int tileWidth = _sourceRectangle.Width;
            int tileHeight = _sourceRectangle.Height;

            // Ground level
            for (int x = 0; x < screenWidth; x += tileWidth)
            {
                _platforms.Add(new Rectangle(x, screenHeight - tileHeight, tileWidth, tileHeight));
            }

            // Floating platforms
            _platforms.Add(new Rectangle(100, screenHeight - tileHeight * 4, tileWidth, tileHeight));
            _platforms.Add(new Rectangle(300, screenHeight - tileHeight * 6, tileWidth, tileHeight));
            _platforms.Add(new Rectangle(500, screenHeight - tileHeight * 8, tileWidth, tileHeight));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var platform in _platforms)
            {
                spriteBatch.Draw(_texture, platform, _sourceRectangle, Color.White);
            }
        }

        public List<Rectangle> GetPlatforms()
        {
            return _platforms;
        }
    }
}
