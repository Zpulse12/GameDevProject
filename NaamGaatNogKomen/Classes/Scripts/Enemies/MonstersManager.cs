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
        private Texture2D Monster2DeathTexture;

        public List<Monster2> Monster2List;
        private Texture2D Monster2Texture;

        public List<Monster3> Monster3List;
        private Texture2D Monster3Texture;
        private Texture2D Monster3ProjectileTexture;

        public MonstersManager()
        {
            //Monster1List = new List<Monster1>();
            //Monster1List.Add(new Monster1());
        }

        public void LoadContent(ContentManager content)
        {
            Monster1Texture = content.Load<Texture2D>("Monster1");
            Monster2Texture = content.Load<Texture2D>("Monster2");
            Monster2DeathTexture = content.Load<Texture2D>("Monster2Death");
            Monster3Texture = content.Load<Texture2D>("Monster3");
            Monster3ProjectileTexture = content.Load<Texture2D>("Projectile");
        }
        public void LoadLevel(int level)
        {
            Dictionary<Vector2, int> MonsterData = MapGenerator.LoadMap($"Map Creation\\Level{level}_Monsters.csv");
            AddMonsters(MonsterData);
        }

        private void AddMonsters(Dictionary<Vector2, int> monstData)
        {
            Monster1List = new List<Monster1>();
            Monster2List = new List<Monster2>();
            Monster3List = new List<Monster3>();

            foreach (var item in monstData)
            {
                if (item.Value == 0) // monster 1 (flying monster)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * MapGenerator.tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * MapGenerator.tileSize * GameManager.gameScale;

                    Monster1List.Add(new Monster1(pos));
                }
                else if (item.Value == 1) // monster 2 (crawling monster)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * MapGenerator.tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * MapGenerator.tileSize * GameManager.gameScale;

                    Monster2List.Add(new Monster2(pos));
                }
                else if (item.Value == 2) // monster 3 (Throwing monster)
                {
                    Vector2 pos;
                    pos.X = item.Key.X * MapGenerator.tileSize * GameManager.gameScale;
                    pos.Y = item.Key.Y * MapGenerator.tileSize * GameManager.gameScale;

                    Monster3List.Add(new Monster3(pos));
                }
            }
        }
        public void Update(float deltaTime, Vector2 knightPos)
        {
            foreach (Monster1 monster in Monster1List)
                monster.Update(deltaTime);
            foreach (Monster2 monster in Monster2List)
                monster.Update(deltaTime);
            foreach (Monster3 monster in Monster3List)
                monster.Update(deltaTime, knightPos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Monster1 monster in Monster1List)
                monster.Draw(spriteBatch, Monster1Texture);
            foreach (Monster2 monster in Monster2List)
                monster.Draw(spriteBatch, Monster2Texture, Monster2DeathTexture);
            foreach (Monster3 monster in Monster3List)
                monster.Draw(spriteBatch, Monster3Texture, Monster3ProjectileTexture);
        }

        public void MoveLeft(int amount)
        {
            foreach (Monster1 monster in Monster1List)
                monster.MoveLeft(amount);
            foreach (Monster2 monster in Monster2List)
                monster.MoveLeft(amount);
            foreach (Monster3 monster in Monster3List)
                monster.MoveLeft(amount);
        }
    }
}
