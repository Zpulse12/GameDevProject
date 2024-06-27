using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.Interfaces
{
    public interface IAnimationFrame
    {
        double Duration { get; }
        object Frame { get; }
    }
}
