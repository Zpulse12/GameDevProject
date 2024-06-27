using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NaamGaatNogKomen
{
    public class GameOverScreen
    {
        private SpriteFont font;
        private bool isGameOver;
        private string gameOverText = "Game Over! Press Enter to Restart";

        public GameOverScreen(SpriteFont font)
        {
            this.font = font;
            isGameOver = false;
        }

        public bool IsGameOver
        {
            get { return isGameOver; }
            set { isGameOver = value; }
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isGameOver = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (isGameOver)
            {
                spriteBatch.Begin();

                Vector2 textSize = font.MeasureString(gameOverText);
                Vector2 position = new Vector2((graphics.PreferredBackBufferWidth - textSize.X) / 2, (graphics.PreferredBackBufferHeight - textSize.Y) / 2);
                spriteBatch.DrawString(font, gameOverText, position, Color.White);

                spriteBatch.End();
            }
        }
    }
}
