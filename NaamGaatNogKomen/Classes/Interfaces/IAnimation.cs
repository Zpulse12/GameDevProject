using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.Interfaces
{
    public interface IAnimation
    {
        void AddFrame(IAnimationFrame frame);
        void Update(double deltaTime);
        IAnimationFrame GetCurrentFrame();
    }
}
