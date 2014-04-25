using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Render
{
    //Maintains all Drawable components. Query each frame for them with GetDrawables.
    /// <summary>
    /// Maintains all Drawable components.
    /// </summary>
    public class RenderManager
    {
        /// <summary>
        /// The list of RenderComponents that are maintained by the RenderManager.
        /// </summary>
        private List<RenderComponent> Components;

        /// <summary>
        /// A new instance of RenderManager.
        /// </summary>
        public RenderManager()
        {
            Components = new List<RenderComponent>();
        }

        public void Update(int dTime)
        {
            foreach (var comp in Components)
            {
                comp.Update(dTime);
            }
        }

        //TODO: sort Drawables, only return what needs drawing on screen
        /// <summary>
        /// Returns the visible drawables.
        /// </summary>
        /// <returns>The Drawables to draw.</returns>
        public List<RenderComponent> GetDrawables()
        {
            return Components.ToList<RenderComponent>();
        }
    }
}
