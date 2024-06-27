using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaamGaatNogKomen
{
    public class Animation
    {
        public List<Rectangle> Frames { get; private set; }
        public int CurrentFrameIndex { get; private set; }
        public float FrameTime { get; private set; }
        private float time;

        public Rectangle CurrentFrame
        {
            get { return Frames[CurrentFrameIndex]; }
        }

        public void GetFramesFromTexture(int textureWidth, int textureHeight, int frameCountX, int frameCountY)
        {
            Frames = new List<Rectangle>();
            int frameWidth = textureWidth / frameCountX;
            int frameHeight = textureHeight / frameCountY;
            for (int y = 0; y < frameCountY; y++)
            {
                for (int x = 0; x < frameCountX; x++)
                {
                    Frames.Add(new Rectangle(x * frameWidth, y * frameHeight, frameWidth, frameHeight));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time >= FrameTime)
            {
                CurrentFrameIndex = (CurrentFrameIndex + 1) % Frames.Count;
                time = 0f;
            }
        }
    }
}
