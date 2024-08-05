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
        private List<Monster1> Monster1List;
        private Texture2D Monster1Texture;



        public MonstersManager()
        {
            Monster1List = new List<Monster1>();
            Monster1List.Add(new Monster1());
        }

        public void LoadContent(ContentManager content)
        {
            Monster1Texture = content.Load<Texture2D>("Enemy1");
        }

        public void Update(float deltaTime)
        {
            foreach (Monster1 monster in Monster1List)
                monster.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Monster1 monster in Monster1List)
                monster.Draw(spriteBatch, Monster1Texture);
        }

        public void MoveLeft(int amount)
        {
            foreach (Monster1 monster in Monster1List)
                monster.MoveLeft(amount);
        }
    }
}
