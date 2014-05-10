
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Engine.Render;

namespace Jeden.Game
{
    /// <summary>
    /// Sets a GameObject's Valid to false on when a AnimationComponent .IsFinished() equals true.
    /// </summary>
    class InvalidatesWhenAnimationIsFinishedComponent : Component
    {
        AnimationRenderComponent AnimRenderComponent;

        public InvalidatesWhenAnimationIsFinishedComponent(AnimationRenderComponent animComponent, GameObject parent) : base(parent)
        {
            AnimRenderComponent = animComponent;  
        }

        public override void Update(Engine.GameTime gameTime)
        {
            base.Update(gameTime);

            if(AnimRenderComponent.IsFinished())
            {
                Parent.Valid = false;
            }
        }
    }
}
