using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes
{
    internal class GraphicsSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool FullScreen { get; set; }

        public GraphicsSettings(int width, int height, bool fullScreen)
        {
            Width = width;
            Height = height;
            FullScreen = fullScreen;
        }

    }
}
