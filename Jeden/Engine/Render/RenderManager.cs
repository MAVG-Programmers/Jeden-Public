using Jeden.Engine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;

namespace Jeden.Engine.Render
{
    //Maintains all Drawable components. Query each frame for them with GetDrawables.
    /// <summary>
    /// Maintains all Drawable components.
    /// </summary>
    public class RenderManager : Manager 
    {
        public Camera Camera { get; set; }
        
        public RenderTarget Target { get; set; }

        /// <summary>
        /// The list of RenderComponents that are maintained by the RenderManager.
        /// </summary>
        public List<RenderComponent> Components;

        /// <summary>
        /// A new instance of RenderManager.
        /// </summary>
        public RenderManager(/*RenderTarget target*/)
        {
            Components = new List<RenderComponent>();
            Camera = new Camera();
        }

        /// <summary>
        /// Update tick all components owned by this manager
        /// </summary>
        /// <param name="gameTime"></param>

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            foreach (var comp in Components)
            {
                comp.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all of the RenderComponents
        /// </summary>
        public void Draw()
        {
            Target.SetView(Camera);
            
            List<RenderComponent> visibles = new List<RenderComponent>();

            foreach (RenderComponent rComp in Components)
            {
                if(Camera.ViewRect.Intersects(rComp.GetScreenRect(Camera)))
                    visibles.Add(rComp);
            }

            //visibles.Sort(new TextureComparer());
            visibles.Sort(new ZComparer());

            foreach(RenderComponent visible in visibles)
                visible.Draw(this, Camera);


        }

        class ZComparer : IComparer<RenderComponent>
        {
            public int Compare(RenderComponent a, RenderComponent b)
            {
                if (a.ZIndex > b.ZIndex)
                    return 1;
                if (a.ZIndex < b.ZIndex)
                    return -1;
                return 0;
            }
        }

        public void DrawSprite(Texture texture,
                         IntRect subImageRect,
                         Vector2f centerPos,
                         float viewWidth,
                         float viewHeight,
                         bool flipX,
                         bool flipY,
                         Color tint,
                         int zIndex)
        {
            Vertex[] vertices = new Vertex[4];

            float hw = viewWidth / 2.0f;
            float hh = viewHeight / 2.0f;

            vertices[0].Position = new Vector2f(-hw, +hh);
            vertices[1].Position = new Vector2f(-hw, -hh);
            vertices[2].Position = new Vector2f(+hw, -hh);
            vertices[3].Position = new Vector2f(+hw, +hh);

            if (flipX)
            {
                vertices[0].TexCoords.X = subImageRect.Left + subImageRect.Width;
                vertices[1].TexCoords.X = subImageRect.Left + subImageRect.Width;
                vertices[2].TexCoords.X = subImageRect.Left;
                vertices[3].TexCoords.X = subImageRect.Left;
            }
            else
            {
                vertices[0].TexCoords.X = subImageRect.Left;
                vertices[1].TexCoords.X = subImageRect.Left;
                vertices[2].TexCoords.X = subImageRect.Left + subImageRect.Width;
                vertices[3].TexCoords.X = subImageRect.Left + subImageRect.Width;
            }

            if (flipY)
            {
                vertices[0].TexCoords.Y = subImageRect.Top;
                vertices[1].TexCoords.Y = subImageRect.Top + subImageRect.Height;
                vertices[2].TexCoords.Y = subImageRect.Top + subImageRect.Height;
                vertices[3].TexCoords.Y = subImageRect.Top;
            }
            else
            {
                vertices[0].TexCoords.Y = subImageRect.Top + subImageRect.Height;
                vertices[1].TexCoords.Y = subImageRect.Top;
                vertices[2].TexCoords.Y = subImageRect.Top;
                vertices[3].TexCoords.Y = subImageRect.Top + subImageRect.Height;
            }


            for (int i = 0; i < 4; i++)
            {
                vertices[i].Position += centerPos;
            }

            vertices[0].Color = tint;
            vertices[1].Color = tint;
            vertices[2].Color = tint;
            vertices[3].Color = tint;

            RenderStates rs = new RenderStates();
            rs.BlendMode = BlendMode.Alpha;
            rs.Texture = texture;
            rs.Transform = Transform.Identity;
            rs.Shader = null;

            Target.Draw(vertices, PrimitiveType.Quads, rs);

        }

        /// <summary>
        /// Removes a render component from the list
        /// </summary>
        /// <param name="comp">The component to remove</param>
        public override void RemoveComponent(Component comp)
        {
            Debug.Assert(comp is RenderComponent);

            if(Components.Contains(comp))
                Components.Remove((RenderComponent)comp);
        }


        float Vector2Dot(Vector2f x, Vector2f y) // temporary, this need to be global
        {
            return x.X * y.X + x.Y * y.Y;
        }

        /// <summary>
        ///Creates a single fixed image component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public SpriteRenderComponent MakeNewSpriteComponent(GameObject owner, Texture texture)
        {
            SpriteRenderComponent component = new SpriteRenderComponent(this, owner, texture);
            Components.Add(component);
            return component;
        }

        /// <summary>
        ///Creates a set of images component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public SpriteSetRenderComponent MakeNewSpriteSetComponent(GameObject owner)
        {
            SpriteSetRenderComponent component = new SpriteSetRenderComponent(this, owner);
            Components.Add(component);
            return component;
        }

        /// <summary>
        ///Creates a single animation component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public AnimationRenderComponent MakeNewAnimationComponent(GameObject owner)
        {
            AnimationRenderComponent component = new AnimationRenderComponent(this, owner);
            Components.Add(component);
            return component;
        }

        /// <summary>
        ///Creates a set of animations component for a GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public AnimationSetRenderComponent MakeNewAnimationSetComponent(GameObject owner)
        {
            AnimationSetRenderComponent component = new AnimationSetRenderComponent(this, owner);
            Components.Add(component);
            return component;
        }

        /// <summary>
        ///Creates a parallax image for the GameObject to use
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public ParallaxRenderComponent MakeNewParallaxComponent(GameObject owner, Texture texture, float factor)
        {
            ParallaxRenderComponent component = new ParallaxRenderComponent(this, owner, texture, factor);
            Components.Add(component);
            return component;
        }

        /// <summary>
        ///Creates a new tile map component for the GameObject to use, this still need to be loaded with data from the tile map loader
        ///Also adds the new component to the list of those in use
        ///</summary>
        ///<returns> The new component.</returns>
        public TileMapRenderComponent MakeNewTileMapComponent(GameObject owner)
        {
            TileMapRenderComponent component = new TileMapRenderComponent(this, owner);
            Components.Add(component);
            return component;
        }

    }
}
