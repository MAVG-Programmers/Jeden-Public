
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using SFML.Graphics;
using SFML.Window;

namespace Jeden.Engine.Render
{
    public class ParallaxRenderComponent : SpriteRenderComponent
    {
        /// <summary>
        /// The amount that movement relative to the camera is scaled by
        /// </summary>
        public float ParallaxFactor { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The GameObject that owns the component</param>
        /// <param name="texture">The texture that is drawn</param>
        /// <param name="factor">The parallax factor, the amount which the movement is scaled relative to the camera</param>
        public ParallaxRenderComponent(RenderManager renderMgr, GameObject parent, Texture texture, float factor)
            : base(renderMgr, parent, texture)
        {
            ParallaxFactor = factor;
        }

        /// <summary>
        /// Returns the clip rect
        /// </summary>
        /// <param name="camera">The active camera</param>
        /// <returns>The clip rect</returns>
        public override FloatRect GetScreenRect(Camera camera)
        {
            Vector2f pos = (WorldPosition - camera.Center) * ParallaxFactor + camera.Center;
            
            FloatRect rect;
            
            rect.Left = pos.X - WorldWidth * 0.5f;
            rect.Top = pos.Y - WorldHeight * 0.5f;
            rect.Width = WorldWidth;
            rect.Height = WorldHeight;

            return rect;
        }

        /// <summary>
        /// Draw it.
        /// </summary>
        /// <param name="renderMgr"></param>
        /// <param name="camera"></param>
        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            //add camera position to where we want to draw on the screen, and let the view transform just subtract it back off
            renderMgr.DrawSprite(Texture, SubImageRect, (WorldPosition - camera.Center) * ParallaxFactor + camera.Center, 
                WorldWidth, WorldHeight, FlipX, FlipY, Tint, ZIndex);
        }
    }
}
