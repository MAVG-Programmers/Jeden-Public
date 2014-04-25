using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Jeden.Engine.Render
{
    public class RenderComponent : Drawable
    {
        //TODO: Draw, add textures once resource management works
        //Drawable to window
        /// <summary>
        /// Draws the GameObject's sprites.
        /// </summary>
        /// <param name="target">The render target to be drawn onto.</param>
        /// <param name="states">The render state.</param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            
        }
    }
}
