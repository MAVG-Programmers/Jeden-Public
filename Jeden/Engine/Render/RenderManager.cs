using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Render
{
    //Maintains all drawable components. Query each frame for them with GetDrawables.
    public class RenderManager
    {
        private List<RenderComponent> Components;

        public RenderManager()
        {
            Components = new List<RenderComponent>();
        }

        //TODO: sort drawables, only return what needs drawing on screen
        public List<RenderComponent> GetDrawables()
        {
            return Components.ToList<RenderComponent>();
        }
    }
}
