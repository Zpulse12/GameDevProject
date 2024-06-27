using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NaamGaatNogKomen.Classes
{
    internal class Animation
    {
        public AnimationFrame CurrentFrame { get; private set; }
        private List<AnimationFrame> frames;
        private int counter;
        private double secondCounter = 0;

        public Animation()
        {
            frames = new List<AnimationFrame>();
        }

        public void AddFrame(AnimationFrame frame)
        {
            frames.Add(frame);
            if (frames.Count == 1)
            {
                CurrentFrame = frames[0];
            }
        }

        public void Update(GameTime gameTime)
        {
            if (frames.Count == 0)
            {
                return;
            }

            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            int fps = 15;

            if (secondCounter >= 1d / fps)
            {
                counter++;
                secondCounter = 0;
            }
            if (counter >= frames.Count)
            {
                counter = 0;
            }

            CurrentFrame = frames[counter];
        }

        public void GetFramesFromTexture(int width, int height, int numberOfWidthSprites, int numberOfHeightSprites)
        {
            int widthOfFrame = width / numberOfWidthSprites;
            int heightOfFrame = height / numberOfHeightSprites;

            for (int y = 0; y <= height - heightOfFrame; y += heightOfFrame)
            {
                for (int x = 0; x <= width - widthOfFrame; x += widthOfFrame)
                {
                    AddFrame(new AnimationFrame(new Rectangle(x, y, widthOfFrame, heightOfFrame)));
                }
            }
        }
    }
}
