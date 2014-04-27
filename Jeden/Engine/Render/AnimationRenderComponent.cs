using Jeden.Engine.Object;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Render
{
    class AnimationRenderComponent : RenderComponent 
    {
        Animation Animation;

        AnimationRenderComponent(GameObject parent) : base(parent) 
        {
            Animation = new Animation();
        }

        void AddFrame(Texture texture, IntRect subImageRect)
        {
            Animation.AddFrame(texture, subImageRect);
        }

        public override void Draw(Renderer renderer)
        {
            Animation.Draw(renderer, Position, ViewWidth, ViewHeight, Angle, RotationCenter, FlipX, FlipY, Tint);
        }


    }
}
