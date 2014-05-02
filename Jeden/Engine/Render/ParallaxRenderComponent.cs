
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
        public float ParallaxFactor { get; set; }

        public ParallaxRenderComponent(GameObject parent, Texture texture, float factor)
            : base(parent, texture)
        {
            ParallaxFactor = factor;
        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            //add camera position to where we want to draw on the screen, and let the view transform just subtract it back off
            renderMgr.DrawSprite(Texture, SubImageRect, (Position - camera.Center) * ParallaxFactor + camera.Center, 
                WorldWidth, WorldHeight, Angle, RotationCenter, FlipX, FlipY, Tint, ZIndex);
        }
    }
}
