using Microsoft.Xna.Framework.Input;
using NaamGaatNogKomen.Classes.Scripts.Hero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKnight.Classes.Interfaces
{
    internal interface IKnightState
    {
        void Enter(Knight knight, float deltaTime = 0);
        void HandleInput(Knight knight, KeyboardState keyboardState);
        void Update(Knight knight, float deltaTime);
        void PlayAnimation(Knight knight, float deltaTime);
    }
}
