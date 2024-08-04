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

        private Dictionary<Vector2, int> Level1Data;
        public List<Hitbox> colliders;
        private static readonly int tileSize = 16;
        private static readonly int tilesetWidth = 19;



        public MapGenerator()
        {
            string dir = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin"));
            Level1Data = LoadMap(dir + $"Map Creation\\Level1_Platform.csv");
            colliders = AddColliders(LoadMap(dir + $"Map Creation\\Level1_Colliders.csv"));

        }

        public void LoadContent(ContentManager content)
        {
            tileset = content.Load<Texture2D>("Castle_tileset");
        }

        public void Update(float deltaTime)
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Level1Data)
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
        private List<Hitbox> AddColliders(Dictionary<Vector2, int> colData)
        {
            List<Hitbox> colList = new List<Hitbox>();

            foreach (var item in colData)
            {
                if (item.Value == 0)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * tileSize * GameManager.gameScale;

                    Rectangle r = new Rectangle((int)pos.X, (int)pos.Y,
                            (int)(tileSize * GameManager.gameScale), (int)(tileSize * GameManager.gameScale));
                    colList.Add(new Hitbox(r, new Vector2(0, 0)));
                }
            }

            return colList;
        }

        public void MoveLeft(int amount)
        {
            mapPosition.X -= amount;
            foreach (Hitbox collider in colliders)
                collider.MoveLeft(amount);
        }
    }
}
