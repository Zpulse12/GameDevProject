using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using System.Reflection.Metadata;
using System.Reflection.Emit;
using NaamGaatNogKomen.Classes.Scripts.Enemies;

namespace NaamGaatNogKomen.Classes.Scripts.Managers
{
    enum GameState
    {
        StartMenu,
        Level1,
        Level2,
        GameOver_Loss,
        GameOver_Win,

    }

    enum MenuState
    {
        Main,
        HowToPlay,
        Exit
    }
    internal class GameManager
    {
        private static Knight knight;
        private static MapGenerator mapGenerator;
        private static MonstersManager monstersManager;

        private int menuSelection;
        public MenuState menuState;
        private SpriteFont spriteFont;
        private SpriteFont spriteLargeFont;
        private static GameState gameState;
        private KeyboardState previousKeyboardState;
        private string[] gameOver = { "Restart", "Exit" };
        private string[] main = { "Start Game", "How to play", "Exit" };
        private string[] howToPlay = { "Move left: left arrow", "Move right: right arrow", "Jump: up arrow or Space", "Back" };
        public static float scrollAmount;

        //Hearts Texture
        private static Texture2D plainTexture;

        private static int level;
        private static int lives;
        private static float deathTimer;

        private readonly float deathGapTime = 5;
        public static readonly float gameScale = 3f;
        public static readonly int mapWidth = (int)(24 * MapGenerator.tileSize * gameScale);
        public static readonly int mapHeight = (int)(16 * MapGenerator.tileSize * gameScale);


        public GameManager(GraphicsDeviceManager graphics)
        {
            knight = new Knight();
            mapGenerator = new MapGenerator();
            monstersManager = new MonstersManager();
            plainTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            plainTexture.SetData(new Color[] { Color.White });
            gameState = GameState.StartMenu;
            menuState = MenuState.Main;
        }


        public void LoadContent(ContentManager content)
        {
            knight.LoadContent(content);
            mapGenerator.LoadContent(content);
            monstersManager.LoadContent(content);
            spriteFont = content.Load<SpriteFont>("Fonts/Font");
            spriteLargeFont = content.Load<SpriteFont>("Fonts/FontLarge");
            //GoToNextLevel();
        }



        public void Update(float deltaTime)
        {
            switch (gameState)
            {
                case GameState.StartMenu:
                    KeyboardState keyboardState = Keyboard.GetState();

                    switch (menuState)
                    {
                        case MenuState.Main:
                            if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                                menuSelection = menuSelection < 2 ? menuSelection + 1 : 0;
                            else if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                                menuSelection = menuSelection > 0 ? menuSelection - 1 : 2;
                            else if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                            {
                                switch (menuSelection)
                                {
                                    case 0:
                                        level = 0;
                                        lives = 3;
                                        GoToNextLevel();
                                        gameState = GameState.Level1;
                                        break;
                                    case 1:
                                        menuState = MenuState.HowToPlay;
                                        menuSelection = 3;
                                        break;
                                    case 2:
                                        menuState = MenuState.Exit;
                                        return;
                                }
                            }
                            break;

                        case MenuState.HowToPlay:
                            if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                            {
                                menuState = MenuState.Main;
                                menuSelection = 0;
                            }
                            break;
                        default:
                            break;
                    }

                    previousKeyboardState = keyboardState;
                    break;

                case GameState.Level1:
                    knight.Update(deltaTime);
                    mapGenerator.Update(deltaTime, 1);
                    monstersManager.Update(deltaTime, knight.GetPostion());
                    if (TouchedFinishLine(knight.hitbox))
                    {
                        scrollAmount = 0;
                        gameState = GameState.Level2;
                        GoToNextLevel();
                    }

                    break;

                case GameState.Level2:
                    knight.Update(deltaTime);
                    mapGenerator.Update(deltaTime, 2);
                    monstersManager.Update(deltaTime, knight.GetPostion());
                    if (TouchedFinishLine(knight.hitbox))
                    {
                        scrollAmount = 0;
                        gameState = GameState.GameOver_Win;
                    }
                    break;

                case GameState.GameOver_Loss:
                    if (deathTimer < deathGapTime)
                    {
                        deathTimer += deltaTime;
                        knight.Update(deltaTime);
                        mapGenerator.Update(deltaTime, level);
                        monstersManager.Update(deltaTime, knight.GetPostion());
                    }
                    else
                    {
                        keyboardState = Keyboard.GetState();

                        if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                            menuSelection = menuSelection < 1 ? menuSelection + 1 : 0;
                        else if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                            menuSelection = menuSelection > 0 ? menuSelection - 1 : 1;
                        else if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                        {
                            switch (menuSelection)
                            {
                                case 0:
                                    level = 0;
                                    lives = 3;
                                    scrollAmount = 0;
                                    gameState = GameState.Level1;
                                    GoToNextLevel();
                                    break;
                                case 1:
                                    menuState = MenuState.Exit; // Exit game
                                    break;
                            }
                        }
                        previousKeyboardState = keyboardState;
                    }


                    break;
                case GameState.GameOver_Win:
                    keyboardState = Keyboard.GetState();

                    if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                        menuSelection = menuSelection < 1 ? menuSelection + 1 : 0;
                    else if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                        menuSelection = menuSelection > 0 ? menuSelection - 1 : 1;
                    else if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        switch (menuSelection)
                        {
                            case 0:
                                level = 0;
                                lives = 3;
                                scrollAmount = 0;
                                gameState = GameState.Level1;
                                GoToNextLevel();
                                break;
                            case 1:
                                menuState = MenuState.Exit; // Exit game
                                break;
                        }
                    }
                    previousKeyboardState = keyboardState;

                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.StartMenu:
                    // draw for start menu

                    switch (menuState)
                    {
                        case MenuState.Main:
                            DrawMenuOptions(spriteBatch, main, 100);
                            break;
                        case MenuState.HowToPlay:
                            DrawMenuOptions(spriteBatch, howToPlay, 90);
                            break;
                        default:
                            break;

                    }
                    break;

                case GameState.Level1:
                    // draw for level 1
                    mapGenerator.DrawBackground(spriteBatch, 1);

                    for (int i = 0; i < lives; ++i)
                        DrawPixelHeart(spriteBatch, (int)scrollAmount + (int)(10 * gameScale) + (int)(12 * gameScale) * i,
                            (int)(0.9f * MapGenerator.tileSize * gameScale), (int)gameScale, Color.DarkRed);

                    monstersManager.Draw(spriteBatch);
                    mapGenerator.DrawPlatform(spriteBatch, 1);
                    knight.Draw(spriteBatch);
                    break;

                case GameState.Level2:
                    // draw for level 2
                    mapGenerator.DrawBackground(spriteBatch, 2);

                    for (int i = 0; i < lives; ++i)
                        DrawPixelHeart(spriteBatch, (int)scrollAmount + (int)(10 * gameScale) + (int)(12 * gameScale) * i,
                            (int)(0.9f * MapGenerator.tileSize * gameScale), (int)gameScale, Color.DarkRed);

                    monstersManager.Draw(spriteBatch);
                    mapGenerator.DrawPlatform(spriteBatch, 2);
                    knight.Draw(spriteBatch);
                    break;

                case GameState.GameOver_Loss:
                    if (deathTimer < deathGapTime)
                    {
                        // draw for the current level
                        mapGenerator.DrawBackground(spriteBatch, level);

                        for (int i = 0; i < lives; ++i)
                            DrawPixelHeart(spriteBatch, (int)scrollAmount + (int)(10 * gameScale) + (int)(12 * gameScale) * i,
                                (int)(0.9f * MapGenerator.tileSize * gameScale), (int)gameScale, Color.DarkRed);

                        monstersManager.Draw(spriteBatch);
                        mapGenerator.DrawPlatform(spriteBatch, level);
                        knight.Draw(spriteBatch);
                    }
                    // draw for game over menu
                    else
                    {
                        scrollAmount = 0;
                        DrawText(spriteBatch, "You Lost");
                        DrawMenuOptions(spriteBatch, gameOver);
                    }

                    break;

                case GameState.GameOver_Win:
                    // draw for game over menu
                    scrollAmount = 0;
                    DrawText(spriteBatch, "You Won");
                    DrawMenuOptions(spriteBatch, gameOver);

                    break;

                default:
                    break;
            }

        }
        public static void MoveMapLeft(float amount)
        {

            scrollAmount = amount;
            //monstersManager.MoveLeft((int)amount);
        }
        public static int HitMap(Hitbox hitbox, bool withX, bool withY)
        {
            foreach (Hitbox collider in mapGenerator.colliders)
            {
                if (hitbox.rectangle.Intersects(collider.rectangle))
                {
                    int overlapX = Math.Max(0, Math.Min(hitbox.rectangle.Right, collider.rectangle.Right) -
                        Math.Max(hitbox.rectangle.Left, collider.rectangle.Left));

                    int overlapY = Math.Max(0, Math.Min(hitbox.rectangle.Bottom, collider.rectangle.Bottom) -
                        Math.Max(hitbox.rectangle.Top, collider.rectangle.Top));

                    if (withX && !withY && overlapX > 0 && overlapY > 0)
                        return overlapX;
                    else if (withY && !withX && overlapY > 0 && overlapY > 0)
                        return overlapY;
                    else if (withY && withX && overlapX > 0 && overlapY > 0)
                        return 1;
                }
            }
            return 0;
        }

        public static bool HitSpikes(Hitbox hitbox)
        {
            foreach (Hitbox collider in mapGenerator.spikes)
            {
                if (hitbox.rectangle.Intersects(collider.rectangle))
                {
                    --lives;

                    if (lives == 0)
                        DeathRoutine();
                    return true;
                }
            }
            return false;
        }
        public static bool HitMonster(Hitbox hitbox, KnightMovementStates knightMovementStates, bool isInvincible)
        {
            if (!isInvincible)
            {
                foreach (var monster in monstersManager.MonsterList)
                {
                    if (monster is Monster1)
                    {
                        if (hitbox.rectangle.Intersects(monster.hitbox.rectangle))
                        {
                            --lives;

                            if (lives == 0)
                                DeathRoutine();

                            return true;
                        }
                    }
                    else if (monster is Monster2 monster2)
                    {
                        if (hitbox.rectangle.Intersects(monster.hitbox.rectangle) && monster2.IsAlive())
                        {
                            if (knightMovementStates == KnightMovementStates.Fall)
                            {
                                monster2.Die();
                                knight.Bounce();
                            }
                            else if (!isInvincible)
                            {
                                --lives;

                                if (lives == 0)
                                    DeathRoutine();

                                return true;
                            }
                        }
                    }
                    else if (monster is Monster3 monster3)
                    {
                        if (hitbox.rectangle.Intersects(monster.hitbox.rectangle) ||
                        hitbox.rectangle.Intersects(monster3.projectile.hitbox.rectangle))
                        {
                            --lives;

                            if (lives == 0)
                                DeathRoutine();

                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public static bool TouchedFinishLine(Hitbox hitbox)
        {
            if (hitbox.rectangle.Intersects(mapGenerator.finishLine.rectangle))
                return true;

            return false;
        }

        public static void GoToNextLevel()
        {
            scrollAmount = 0;
            level++;
            mapGenerator.LoadLevel(level);
            monstersManager.LoadLevel(level);
            knight.GoToInitialPosition(level);
        }

        public static void DrawPixelHeart(SpriteBatch spriteBatch, int x, int y, int size, Color color)
        {

            Rectangle[] heart = new Rectangle[]
            {
                new Rectangle(x + size * 1, y + size * 0, size * 2, size),
                new Rectangle(x + size * 6, y + size * 0, size * 2, size),

                new Rectangle(x + size * 0, y + size * 1, size * 4, size),
                new Rectangle(x + size * 5, y + size * 1, size * 4, size),

                new Rectangle(x + size * 0, y + size * 2, size * 9, size * 2),
                new Rectangle(x + size * 1, y + size * 4, size * 7, size * 1),
                new Rectangle(x + size * 2, y + size * 5, size * 5, size * 1),
                new Rectangle(x + size * 3, y + size * 6, size * 3, size * 1),
                new Rectangle(x + size * 4, y + size * 7, size * 1, size * 1),
            };

            foreach (Rectangle rect in heart)
                spriteBatch.Draw(plainTexture, rect, color);
        }
        private void DrawMenuOptions(SpriteBatch spriteBatch, string[] menuOptions, int Y = 160)
        {
            Vector2 position = new Vector2(mapWidth / 2, Y * gameScale); // Adjust as needed

            for (int i = 0; i < menuOptions.Length; i++)
            {
                Color textColor = i == menuSelection ? Color.Red : Color.White;
                float wordDistX = position.X - 0.5f * spriteFont.MeasureString(menuOptions[i]).X;
                Vector2 wordPos = new Vector2(wordDistX, position.Y);

                spriteBatch.DrawString(spriteFont, menuOptions[i], wordPos, textColor);

                position.Y += 17 * gameScale; // Adjust spacing between options
            }
        }
        private void DrawText(SpriteBatch spriteBatch, string s)
        {
            Vector2 position = new Vector2(200, 100) * gameScale;
            float wordDistX = position.X - 0.5f * spriteLargeFont.MeasureString(s).X;
            Vector2 wordPos = new Vector2(wordDistX, position.Y);
            spriteBatch.DrawString(spriteLargeFont, s, wordPos, Color.White);
            DrawMenuOptions(spriteBatch, gameOver);
        }

        private static void DeathRoutine()
        {
            deathTimer = 0;
            gameState = GameState.GameOver_Loss;
            knight.DeathRoutine();
        }
    }

}
