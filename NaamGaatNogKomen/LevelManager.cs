using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaamGaatNogKomen
{
    public class LevelManager
    {
        private List<Level> levels;
        private int currentLevelIndex;

        public LevelManager(List<Level> levels)
        {
            this.levels = levels;
            currentLevelIndex = 0;
        }

        public Level CurrentLevel => levels[currentLevelIndex]; // Eigenschap om de huidige level te krijgen

        public void NextLevel()
        {
            if (currentLevelIndex < levels.Count - 1)
            {
                currentLevelIndex++;
            }
            else
            {
                // Handle end of game or loop back to first level
                currentLevelIndex = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentLevel.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            CurrentLevel.Update(gameTime);
        }
    }
}
