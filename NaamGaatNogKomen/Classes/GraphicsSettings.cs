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
        private GraphicsDeviceManager _graphics;

        public GraphicsSettings(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public void ApplyFullscreenSettings()
        {
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
        }

    }
}
