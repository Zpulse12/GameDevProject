using NaamGaatNogKomen.Classes.Scripts.Enemies;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.Scripts.Managers
{
    internal class MonsterFactory
    {
        public Enemy CreateMonster(int type, Vector2 pos)
        {
            switch (type)
            {
                case 0:
                    return new Monster1(pos);
                case 1:
                    return new Monster2(pos);
                case 2:
                    return new Monster3(pos);
                default:
                    throw new ArgumentException("Invalid type");
            }
        }
    }
}
