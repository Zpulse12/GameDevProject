using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NaamGaatNogKomen
{
    public class StartScreen
    {
        private SpriteFont font;
        private bool isGameStarted;
        private string startText = "Press Enter to Start";

        public StartScreen(SpriteFont font)
        {
            this.font = font;
            isGameStarted = false;
        }

        public bool IsGameStarted
        {
            get { return isGameStarted; }
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isGameStarted = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            Vector2 textSize = font.MeasureString(startText);
            Vector2 position = new Vector2((graphics.PreferredBackBufferWidth - textSize.X) / 2, (graphics.PreferredBackBufferHeight - textSize.Y) / 2);
            spriteBatch.DrawString(font, startText, position, Color.White);

            spriteBatch.End();
        }
    }
}
