using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace NaamGaatNogKomen.Classes.Input
{
    public class KeyboardInputReader : IInputReader
    {
        public Vector2 ReadInput()
        {
            Vector2 inputDirection = Vector2.Zero;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
                inputDirection.X -= 1;
            if (keyboardState.IsKeyDown(Keys.Right))
                inputDirection.X += 1;
            if (keyboardState.IsKeyDown(Keys.Space))
                inputDirection.Y -= 1; // Use Y-axis for jumping

            return inputDirection;
        }
    }
}
