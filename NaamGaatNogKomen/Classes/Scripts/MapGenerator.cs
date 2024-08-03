using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using static System.Formats.Asn1.AsnWriter;

namespace NaamGaatNogKomen.Classes.Scripts
{
    internal class MapGenerator
    {

        private Texture2D tileset;
        private Vector2 mapPosition;

        private Dictionary<Vector2, int> Level1Data;

        private static readonly int tileSize = 16;
        private static readonly int tilesetWidth = 19;



        public MapGenerator()
        {
            Level1Data = LoadMap("Content//Level1_Platform.csv");
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
                Vector2 pos = mapPosition;

                if (item.Value != -1)
                {
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

        public Dictionary<Vector2, int> LoadMap(string filepath)
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
    }
}
