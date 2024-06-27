using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.GameWorld;
using NaamGaatNogKomen.Classes.Input;
using System.Collections.Generic;

namespace NaamGaatNogKomen
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private StartScreen startScreen;
        private LevelManager levelManager;
        private GameOverScreen gameOverScreen;
        private bool isGameOver;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D floorTexture = Content.Load<Texture2D>("tilesset");
            Texture2D heroWalkTexture = Content.Load<Texture2D>("HeroWalk");
            Texture2D heroIdleTexture = Content.Load<Texture2D>("HeroIdle");
            Texture2D backgroundTexture = Content.Load<Texture2D>("background");
            SpriteFont font = Content.Load<SpriteFont>("DefaultFont");

            IInputReader inputReader = new KeyboardReader();

            GameWorld gameWorld = new GameWorld(floorTexture, heroWalkTexture, heroIdleTexture, backgroundTexture, inputReader, font, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, this);
            Level level1 = new Level(gameWorld);

            GameWorld gameWorld2 = new GameWorld(floorTexture, heroWalkTexture, heroIdleTexture, backgroundTexture, inputReader, font, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, this);
            Level level2 = new Level(gameWorld2);

            levelManager = new LevelManager(new List<Level> { level1, level2 });
            startScreen = new StartScreen(font);
            gameOverScreen = new GameOverScreen(font);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (startScreen.IsGameStarted && !isGameOver)
            {
                levelManager.Update(gameTime);
                if (levelManager.CurrentLevel.GameWorld.IsGameOver())
                {
                    isGameOver = true;
                }
            }
            else if (isGameOver)
            {
                gameOverScreen.Update(gameTime);
            }
            else
            {
                startScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (startScreen.IsGameStarted && !isGameOver)
            {
                _spriteBatch.Begin();
                levelManager.Draw(_spriteBatch);
                _spriteBatch.End();
            }
            else if (isGameOver)
            {
                gameOverScreen.Draw(_spriteBatch, _graphics);
            }
            else
            {
                startScreen.Draw(_spriteBatch, _graphics);
            }

            base.Draw(gameTime);
        }
    }
}
