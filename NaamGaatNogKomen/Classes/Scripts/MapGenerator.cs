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

        private Texture2D tileset;
        private Vector2 mapPosition;

        private Dictionary<Vector2, int> level1Data;
        private Dictionary<Vector2, int> level1BackgroundData;

        private Dictionary<Vector2, int> level1CollidersData;
        public List<Hitbox> colliders;
        public List<Hitbox> spikes;
        public Hitbox finishLine;
        private static readonly int tileSize = 16;
        private static readonly int tilesetWidth = 19;



        public MapGenerator()
        {
            string dir = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin"));
            level1Data = LoadMap(dir + $"Map Creation\\Level1_Platform.csv");
            level1CollidersData = LoadMap(dir + $"Map Creation\\Level1_Colliders.csv");
            level1BackgroundData = LoadMap(dir + $"Map Creation\\Level1_Background.csv");
            AddColliders(level1CollidersData);

        }

        public void LoadContent(ContentManager content)
        {
            tileset = content.Load<Texture2D>("Castle_tileset");
        }

        public void Update(float deltaTime)
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in level1Data)
            {

                if (item.Value != -1)
                {
                    Vector2 pos = mapPosition;
                    pos.X += item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y += item.Key.Y * tileSize * GameManager.gameScale;
                    spriteBatch.Draw(tileset, pos,
                                    new Rectangle(item.Value % tilesetWidth * tileSize,
                                                item.Value / tilesetWidth * tileSize,
                                                tileSize, tileSize),
                                    Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                }
            }
            foreach (var item in level1BackgroundData)
            {
                if (item.Value != -1)
                {
                    Vector2 pos = mapPosition;
                    pos.X += item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y += item.Key.Y * tileSize * GameManager.gameScale;
                    spriteBatch.Draw(tileset, pos,
                                    new Rectangle(item.Value % tilesetWidth * tileSize,
                                                item.Value / tilesetWidth * tileSize,
                                                tileSize, tileSize),
                                    Color.White, 0, Vector2.Zero, GameManager.gameScale, SpriteEffects.None, 0);
                }
            }
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
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

        public void MoveLeft(int amount)
        {
            mapPosition.X -= amount;
            foreach (Hitbox collider in colliders)
                collider.MoveLeft(amount);
            foreach (Hitbox collider in spikes)
                collider.MoveLeft(amount);
            finishLine.MoveLeft(amount);

        }
    }
}
