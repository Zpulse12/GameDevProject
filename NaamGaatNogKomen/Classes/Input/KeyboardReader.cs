using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace NaamGaatNogKomen.Classes.Input
{
    internal class KeyboardReader : IInputReader
    {
        public Vector2 ReadInput()
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 direction = Vector2.Zero;
            if (state.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (state.IsKeyDown(Keys.Right))
                direction.X += 1;
            return direction;
        }
    }
}
