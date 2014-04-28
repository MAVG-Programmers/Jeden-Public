﻿using Jeden.Engine.Object;
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
        public Renderer Renderer { get; set; }
        public Camera Camera { get; set; }

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
            Camera = new Camera();
        }

        //Update tick all components owned by this manager
        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

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

        public void Draw()
        {
            Renderer.Target.SetView(Camera);

            Components.Sort(new ZComparer());

            foreach (RenderComponent rComp in Components)
            {
                rComp.Draw(Renderer, Camera);
            }

           
           // Renderer.Draw();
        }

        class ZComparer : IComparer<RenderComponent>
        {
            public int Compare(RenderComponent a, RenderComponent b)
            {
                if (a.ZIndex == b.ZIndex)
                    return 0;
                if (a.ZIndex < b.ZIndex)
                    return -1;
                return 1;
            }
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

        /// <summary>
        ///Creates a set of images component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public SpriteSetRenderComponent MakeNewSpriteSetComponent(GameObject owner)
        {
            SpriteSetRenderComponent component = new SpriteSetRenderComponent(owner);
            Components.Add((RenderComponent)component);
            return component;
        }

        /// <summary>
        ///Creates a single animation component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public AnimationRenderComponent MakeNewAnimationComponent(GameObject owner)
        {
            AnimationRenderComponent component = new AnimationRenderComponent(owner);
            Components.Add((RenderComponent)component);
            return component;
        }

        /// <summary>
        ///Creates a set of animations component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public AnimationSetRenderComponent MakeNewAnimationSetComponent(GameObject owner)
        {
            AnimationSetRenderComponent component = new AnimationSetRenderComponent(owner);
            Components.Add((RenderComponent)component);
            return component;
        }

        /// <summary>
        ///Creates a parallax image for the GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public ParallaxRenderComponent MakeNewParallaxComponent(GameObject owner, Texture texture, float factor)
        {
            ParallaxRenderComponent component = new ParallaxRenderComponent(owner, texture, factor);
            Components.Add((RenderComponent)component);
            return component;
        }


    }
}
