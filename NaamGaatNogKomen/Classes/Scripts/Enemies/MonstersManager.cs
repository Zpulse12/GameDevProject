using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.Scripts.Enemies
{
    internal class MonstersManager
    {
        public List<Monster1> Monster1List;
        private Texture2D Monster1Texture;

        public List<Monster2> Monster2List;
        private Texture2D Monster2Texture;

        public MonstersManager()
        {
            //Monster1List = new List<Monster1>();
            //Monster1List.Add(new Monster1());
        }

        public void LoadContent(ContentManager content)
        {
            Monster1Texture = content.Load<Texture2D>("Enemy1");
            Monster2Texture = content.Load<Texture2D>("Enemy2");
        }
        public void LoadLevel(int level)
        {
            Dictionary<Vector2, int> MonsterData = MapGenerator.LoadMap($"Map Creation\\Level{level}_Enemies.csv");
            AddMonsters(MonsterData);
        }

        private void AddMonsters(Dictionary<Vector2, int> monstData)
        {
            Monster1List = new List<Monster1>();
            Monster2List = new List<Monster2>();

            foreach (var item in monstData)
            {
                if (item.Value == 10) // monster 1 (flying monster)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * MapGenerator.tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * MapGenerator.tileSize * GameManager.gameScale;

                    Monster1List.Add(new Monster1(pos));
                }
                else if (item.Value == 11) // monster 2 (crawling monster)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * MapGenerator.tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * MapGenerator.tileSize * GameManager.gameScale;

                    Monster2List.Add(new Monster2(pos));
                }
            }
        }
        public void Update(float deltaTime)
        {
            foreach (Monster1 monster in Monster1List)
                monster.Update(deltaTime);
            foreach (Monster2 monster in Monster2List)
                monster.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Monster1 monster in Monster1List)
                monster.Draw(spriteBatch, Monster1Texture);
            foreach (Monster2 monster in Monster2List)
                monster.Draw(spriteBatch, Monster2Texture);
        }

        public void MoveLeft(int amount)
        {
            foreach (Monster1 monster in Monster1List)
                monster.MoveLeft(amount);
            foreach (Monster2 monster in Monster2List)
                monster.MoveLeft(amount);
        }
    }
}
