using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.TilesSet
{
    internal class FloorTiles
    {
        public AnimationFrame CurrentFrame { get; set; }
        private List<AnimationFrame> frames;
        public FloorTiles()
        {
            frames = new List<AnimationFrame>();
        }
    }
}
