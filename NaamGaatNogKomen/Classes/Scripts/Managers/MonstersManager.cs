using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NaamGaatNogKomen.Classes.Scripts.Enemies;

namespace NaamGaatNogKomen.Classes.Scripts.Managers
{
    internal class MonstersManager
    {
        public static Texture2D Monster1Texture;
        public static Texture2D Monster2Texture;
        public static Texture2D Monster3Texture;
        public static Texture2D Monster2DeathTexture;
        public static Texture2D Monster3ProjectileTexture;

        private MonsterFactory monsterFactory;

        public List<Enemy> MonsterList;

        public MonstersManager()
        {
            monsterFactory = new MonsterFactory();
        }

        public void LoadContent(ContentManager content)
        {
            Monster1Texture = content.Load<Texture2D>("Monster1");
            Monster2Texture = content.Load<Texture2D>("Monster2");
            Monster3Texture = content.Load<Texture2D>("Monster3");
            Monster2DeathTexture = content.Load<Texture2D>("Monster2Death");
            Monster3ProjectileTexture = content.Load<Texture2D>("Projectile");
        }
        public void LoadLevel(int level)
        {
            string dir = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin"));

            Dictionary<Vector2, int> MonsterData = MapGenerator.LoadMap(dir + $"Map Creation\\Level{level}_Monsters.csv");
            AddMonsters(MonsterData);
        }

        private void AddMonsters(Dictionary<Vector2, int> monstData)
        {

            MonsterList = new List<Enemy>();
            foreach (var item in monstData)
            {
                if (item.Value > -1 && item.Value < 3)
                {
                    Vector2 pos = item.Key * MapGenerator.tileSize * GameManager.gameScale;

                    MonsterList.Add(monsterFactory.CreateMonster(item.Value, pos));
                }
            }
        }
        public void Update(float deltaTime, Vector2 knightPos)
        {
            foreach (Enemy monster in MonsterList)
                monster.Update(deltaTime, knightPos);
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy monster in MonsterList)
                monster.Draw(spriteBatch);
        }
    }
}
