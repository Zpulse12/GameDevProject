using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaamGaatNogKomen.Classes.GameWorld;

namespace NaamGaatNogKomen
{
    public class Level
    {
        public GameWorld GameWorld { get; private set; }

        public Level(GameWorld gameWorld)
        {
            GameWorld = gameWorld;
        }

        public void Update(GameTime gameTime)
        {
            GameWorld.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameWorld.Draw(spriteBatch);
        }
    }
}
