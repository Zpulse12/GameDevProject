using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.Input;
using NaamGaatNogKomen.Classes.TilesSet;
using System.Collections.Generic;
using System.Diagnostics;

namespace NaamGaatNogKomen.Classes.GameWorld
{
    public class GameWorld
    {
        private FloorTiles floorTiles;
        private Hero hero;
        private Texture2D backgroundTexture;
        private int screenWidth;
        private int screenHeight;
        private bool isGameOver;
        private GameOverScreen gameOverScreen;
        private Game game;

        public GameWorld(Texture2D floorTexture, Texture2D heroWalkTexture, Texture2D heroIdleTexture, Texture2D backgroundTexture, IInputReader inputReader, SpriteFont font, int screenWidth, int screenHeight, Game game)
        {
            this.game = game;

            floorTiles = new FloorTiles(floorTexture, 3);
            floorTiles.GenerateTiles(screenWidth, screenHeight);

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
            Rectangle heroBounds = hero.GetBounds();

            bool isColliding = false;

            Debug.WriteLine($"Hero Bounds before collision check: {heroBounds}");

            foreach (var tile in tiles)
            {
                Debug.WriteLine($"Checking collision between Hero Bounds: {heroBounds} and Tile Bounds: {tile}");

                if (heroBounds.Intersects(tile))
                {
                    hero.Position = new Vector2(hero.Position.X, tile.Top - heroBounds.Height);
                    hero.StopFalling();
                    isColliding = true;
                    Debug.WriteLine("Collision detected. Hero position adjusted.");
                    break;
                }
            }

            if (!isColliding)
            {
                hero.isFalling = true;
                Debug.WriteLine("Hero is falling. Position adjusted.");
            }

            Debug.WriteLine($"Hero Position after collision check: {hero.Position}");
        }


        public void Update(GameTime gameTime)
        {
            if (isGameOver)
            {
                gameOverScreen.Update(gameTime);
                if (!gameOverScreen.IsGameOver)
                {
                    floorTiles.GenerateTiles(screenWidth, screenHeight);
                    Rectangle firstTile = floorTiles.GetTiles()[0];
                    hero.Position = new Vector2(firstTile.X, 610);
                    isGameOver = false;
                }
                return;
            }

            hero.Update(gameTime);
            HandleCollisions();
            CheckGameOver();

            Debug.WriteLine($"Hero Position after Update: {hero.Position}");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            floorTiles.Draw(spriteBatch);
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
