using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace NaamGaatNogKomen.Classes.GameWorld
{
    public class GameWorld
    {
        private FloorTiles floorTiles;
        private Hero hero;
        private Platform platform;
        private Texture2D backgroundTexture;
        private int screenWidth;
        private int screenHeight;
        private bool isGameOver;
        private GameOverScreen gameOverScreen;
        private Game game;

        public GameWorld(Texture2D floorTexture, Texture2D heroWalkTexture, Texture2D heroIdleTexture, Texture2D platformTexture, Texture2D backgroundTexture, IInputReader inputReader, SpriteFont font, int screenWidth, int screenHeight, Game game)
        {
            this.game = game;

            // Floor tiles setup
            Rectangle floorTileSourceRect = new Rectangle(0, 0, 32, 32);
            floorTiles = new FloorTiles(floorTexture, 32, 32, floorTileSourceRect);
            floorTiles.GenerateTiles(screenWidth, screenHeight);

            // Platform setup
            Rectangle platformTileSourceRect = new Rectangle(32, 0, 32, 32);
            platform = new Platform(platformTexture, platformTileSourceRect);
            platform.GeneratePlatforms(screenWidth, screenHeight);

            // Hero setup
            hero = new Hero(heroWalkTexture, heroIdleTexture, inputReader);
            hero.SetScreenSize(screenWidth, screenHeight, floorTiles.TileHeight);

            Rectangle firstTile = floorTiles.GetTiles()[0];
            hero.Position = new Vector2(firstTile.X, firstTile.Y - hero.GetBounds().Height);

            this.backgroundTexture = backgroundTexture;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.isGameOver = false;
            this.gameOverScreen = new GameOverScreen(font);

            Debug.WriteLine($"Hero Spawn Position: {hero.Position}");
            Debug.WriteLine($"First Tile Position: {firstTile.X}, {firstTile.Y}");
        }

        private void HandleCollisions()
        {
            List<Rectangle> tiles = floorTiles.GetTiles();
            List<Rectangle> platforms = platform.GetPlatforms();
            Rectangle heroBounds = hero.GetBounds();

            bool isColliding = false;


            foreach (var tile in tiles)
            {

                if (heroBounds.Intersects(tile))
                {
                    hero.Position = new Vector2(hero.Position.X, tile.Top - heroBounds.Height);
                    hero.StopFalling();
                    isColliding = true;
                    break;
                }
            }

            foreach (var platform in platforms)
            {
                if (heroBounds.Intersects(platform))
                {
                    if (hero.VerticalSpeed > 0) // Falling down
                    {
                        hero.Position = new Vector2(hero.Position.X, platform.Top - heroBounds.Height);
                        hero.StopFalling();
                        isColliding = true;
                        break;
                    }
                }
            }

            if (!isColliding)
            {
                hero.IsFalling = true;
            }

        }

        public void Update(GameTime gameTime)
        {
            if (isGameOver)
            {
                gameOverScreen.Update(gameTime);
                if (!gameOverScreen.IsGameOver)
                {
                    floorTiles.GenerateTiles(screenWidth, screenHeight);
                    platform.GeneratePlatforms(screenWidth, screenHeight);
                    Rectangle firstTile = floorTiles.GetTiles()[0];
                    hero.Position = new Vector2(firstTile.X, 610);
                    isGameOver = false;
                }
                return;
            }

            hero.Update(gameTime, platform.GetPlatforms());
            HandleCollisions();
            CheckGameOver();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            floorTiles.Draw(spriteBatch);
            platform.Draw(spriteBatch);
            hero.Draw(spriteBatch);

            if (isGameOver)
            {
                gameOverScreen.Draw(spriteBatch, new GraphicsDeviceManager(game));
            }
        }

        private void CheckGameOver()
        {
            if (hero.Position.Y > screenHeight)
            {
                isGameOver = true;
            }
        }

        public bool IsGameOver()
        {
            return isGameOver;
        }
    }
}
