using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using static System.Formats.Asn1.AsnWriter;
using System;

namespace NaamGaatNogKomen.Classes.Scripts
{
    internal class MapGenerator
    {
        private Texture2D castleTileset;
        private Texture2D swampTileset;
        private Texture2D torchTexture;

        private Dictionary<Vector2, int> levelData;
        private Dictionary<Vector2, int> levelBackgroundData;
        private Dictionary<Vector2, int> levelBackground1Data;
        private Dictionary<Vector2, int> levelStaticBackgroundData;
        private Dictionary<Vector2, int> levelCollidersData;
        private int torchCurrentFrame;
        private float torchAnimationTimer;
        public List<Hitbox> colliders;
        public List<Hitbox> spikes;
        public Hitbox finishLine;
        public static readonly int tileSize = 16;
        private static readonly int level1TilesetWidth = 19;
        private static readonly int level2TilesetWidth = 25;
        private static readonly float torchAnimationDuration = 0.2f;

        public MapGenerator()
        {
            torchCurrentFrame = 0;
            torchAnimationTimer = 0;
        }
        public void LoadContent(ContentManager content)
        {

            castleTileset = content.Load<Texture2D>("Castle_tileset");
            swampTileset = content.Load<Texture2D>("Swamp_tileset");
            torchTexture = content.Load<Texture2D>("Torch");
            LoadLevel(1);
        }
        public void Update(float deltaTime, int level)
        {
            if (level == 1)
            {
                foreach (var item in levelBackgroundData)
                {
                    if (item.Value == 57) // animate torch
                    {
                        if (torchAnimationTimer >= torchAnimationDuration) // time interval between frames
                        {
                            torchCurrentFrame = torchCurrentFrame + 1 >= 3 ? 0 : torchCurrentFrame + 1;
                            torchAnimationTimer = 0;
                        }
                        torchAnimationTimer += deltaTime;
                    }
                }
            }
        }
        public void DrawPlatform(SpriteBatch spriteBatch, int level)
        {
            Texture2D tileset = level == 1 ? castleTileset : swampTileset;
            int tilesetWidth = level == 1 ? level1TilesetWidth : level2TilesetWidth;
            foreach (var item in levelData)
            {
                if (item.Value != -1)
                {
                    Vector2 pos; 
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                    spriteBatch.Draw(tileset, pos,
                                    new Rectangle(item.Value % tilesetWidth * tileSize,
                                                item.Value / tilesetWidth * tileSize,
                                                tileSize, tileSize),
                                    Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                }
            }
        }
        public void DrawBackground(SpriteBatch spriteBatch, int level)
        {
            Texture2D tileset = level == 1 ? castleTileset : swampTileset;
            int tilesetWidth = level == 1 ? level1TilesetWidth : level2TilesetWidth;
            if (level == 2)
            {
                foreach (var item in levelStaticBackgroundData)
                {
                    if (item.Value != -1)
                    {
                        Vector2 pos;
                        pos.X = GameManager.scrollAmount + item.Key.X * tileSize * GameManager.gameScale;
                        pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                        spriteBatch.Draw(tileset, pos,
                                        new Rectangle(item.Value % tilesetWidth * tileSize,
                                                    item.Value / tilesetWidth * tileSize,
                                                    tileSize, tileSize),
                                        Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                    }
                }
            }
            foreach (var item in levelBackgroundData)
            {
                if (item.Value != -1 && item.Value != 57)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                    spriteBatch.Draw(tileset, pos,
                                    new Rectangle(item.Value % tilesetWidth * tileSize,
                                                item.Value / tilesetWidth * tileSize,
                                                tileSize, tileSize),
                                    Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                }
                if (level == 1 && item.Value == 57) // animate torch
                {
                    Vector2 pos;
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                    spriteBatch.Draw(torchTexture, pos,
                                    new Rectangle(torchCurrentFrame * (17 + 1), 0, 17, 17),
                                    Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                }
            }
            if (level == 2)
            {
                foreach (var item in levelBackground1Data)
                {
                    if (item.Value != -1)
                    {
                        Vector2 pos;
                        pos.X = item.Key.X * tileSize * GameManager.gameScale;
                        pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                        spriteBatch.Draw(tileset, pos,
                                        new Rectangle(item.Value % tilesetWidth * tileSize,
                                                    item.Value / tilesetWidth * tileSize,
                                                    tileSize, tileSize),
                                        Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                    }
                }
            }
        }
        public static Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
            using (StreamReader reader = new StreamReader(filepath))
            {
                int y = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split(',');
                    for (int x = 0; x < items.Length; x++)
                    {
                        int value;
                        if (int.TryParse(items[x], out value) && value >= -1)
                            if (int.TryParse(items[x], out value) && value > -1)
                            {
                                result.Add(new Vector2(x, y), value);
                            }
                    }
                    y++;
                }
            }
            return result;
        }
        private void AddColliders(Dictionary<Vector2, int> colData)
        {
            colliders = new List<Hitbox>();
            spikes = new List<Hitbox>();
            foreach (var item in colData)
            {
                Vector2 pos;
                Rectangle r;
                if (item.Value == 0)
                {
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                    r = new Rectangle((int)pos.X, (int)pos.Y,
                            (int)(tileSize * GameManager.gameScale), (int)(tileSize * GameManager.gameScale));
                    colliders.Add(new Hitbox(r, new Vector2(0, 0)));
                }
                else if (item.Value == 1)
                {
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = (item.Key.Y + 0.5625f) * tileSize * GameManager.gameScale;
                    r = new Rectangle((int)pos.X, (int)pos.Y,
                            (int)(tileSize * GameManager.gameScale), (int)(tileSize * GameManager.gameScale * 0.4375f));
                    spikes.Add(new Hitbox(r, new Vector2(0, 0)));
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = (item.Key.Y + 1) * tileSize * GameManager.gameScale;
                    r = new Rectangle((int)pos.X, (int)pos.Y,
                            (int)(tileSize * GameManager.gameScale), (int)(tileSize * GameManager.gameScale));
                    colliders.Add(new Hitbox(r, new Vector2(0, 0)));
                }
                else if (item.Value == 2)
                {
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * tileSize * GameManager.gameScale;
                    r = new Rectangle((int)pos.X, (int)pos.Y,
                            (int)(tileSize * GameManager.gameScale), (int)(tileSize * GameManager.gameScale));
                    finishLine = new Hitbox(r, new Vector2(0, 0));
                }
            }
        }

        public void LoadLevel(int level)
        {
            string dir = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin"));
            levelData = LoadMap(dir + $"Map Creation\\Level{level}_Platform.csv");
            levelCollidersData = LoadMap(dir + $"Map Creation\\Level{level}_Colliders.csv");
            levelBackgroundData = LoadMap(dir + $"Map Creation\\Level{level}_Background.csv");
            if (level == 2)
            {
                levelStaticBackgroundData = LoadMap(dir + $"Map Creation\\Level{level}_StaticBackground.csv");
                levelBackground1Data = LoadMap(dir + $"Map Creation\\Level{level}_Background1.csv");
            }
            AddColliders(levelCollidersData);
        }
    }
}
