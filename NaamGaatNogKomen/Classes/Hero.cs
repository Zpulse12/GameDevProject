using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes
{
    internal class Hero : IGameObject
    {
        private Texture2D texture;
        Animation animation;
        private Vector2 positie = new Vector2(0, 0);
        private Vector2 snelheid = new Vector2(3, 3);

        public Hero(Texture2D texture)
        {
            this.texture = texture;
            animation = new Animation();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 8, 1);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positie, animation.CurrentFrame.SourceRectangle, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            Move();
        }
        private void Move()
        {
            positie += snelheid;
            if (positie.X > 800 - 64 || positie.X < 0)
                snelheid.X *= -1;
            if (positie.Y > 480 - 64 || positie.Y < 0)
                snelheid.Y *= -1;
        }
    }
}
