using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaamGaatNogKomen.Classes.Interfaces
{
    public interface IGameWorld
    {
        void AddGameObject(IGameObject gameObject);
        void Update(double deltaTime);
        void Render(IGraphicsSettings graphicsSettings);
    }
}
