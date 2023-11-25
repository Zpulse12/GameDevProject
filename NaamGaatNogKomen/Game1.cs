using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes;
using NaamGaatNogKomen.Classes.Input;


namespace NaamGaatNogKomen
{
    public class Game1 : Game
    {
        private GraphicsSettings _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _heroWalkTexture;
        private Texture2D _heroIdleTexture;
        private Texture2D _floorTiles;
        private Hero hero;



        public Game1()
        {

            _graphics = new GraphicsSettings(new GraphicsDeviceManager(this));//De constructor accepteert alleen een object van de graphicsdevicemanager


            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.ApplyFullscreenSettings();

            base.Initialize();


            hero = new Hero(_heroWalkTexture, _heroIdleTexture, new KeyboardReader());
            hero.SetScreenSize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);


           

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _heroWalkTexture = Content.Load<Texture2D>("HeroWalk");
            _heroIdleTexture = Content.Load<Texture2D>("HeroIdle");
            _floorTiles = Content.Load<Texture2D>("tilesset");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            hero.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) 
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            hero.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);

            // TODO: Add your drawing code here
        }
    }
}