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
        public Renderer Renderer;
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
            Renderer = new Renderer(null);
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
        ///Creates a single fixed image component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public SpriteRenderComponent MakeNewSpriteComponent(GameObject owner, Texture texture)
        {
            SpriteRenderComponent component = new SpriteRenderComponent(owner, texture);
            Components.Add((RenderComponent)component);
            return component;
        }

        public SpriteSetRenderComponent MakeNewSpriteSetComponent(GameObject owner)
        {
            SpriteSetRenderComponent component = new SpriteSetRenderComponent(owner);
            Components.Add((RenderComponent)component);
            return component;
        }

    }
}
