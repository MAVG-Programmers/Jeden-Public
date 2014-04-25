using Jeden.Engine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

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

        //Update tick all components owned by this manager
        public void Update(GameTime gameTime)
        {
            foreach (var comp in Components)
            {
                comp.Update(gameTime);
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

        /// <summary>
        ///Creates a render component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public RenderComponent MakeNewComponent(GameObject owner, Texture texture)
        {
            RenderComponent component = new RenderComponent(owner, texture);
            Components.Add(component);
            return component;
        }
    }
}
