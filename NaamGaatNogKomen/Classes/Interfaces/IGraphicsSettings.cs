using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.Interfaces
{
    public interface IGraphicsSettings
    {
        int Width { get; }
        int Height { get; }
        bool FullScreen { get; }
    }
}
