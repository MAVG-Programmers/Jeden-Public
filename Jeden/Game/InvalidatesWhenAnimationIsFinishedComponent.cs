
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine.Object;
using Jeden.Engine.Render;

namespace Jeden.Game
{
    /// <summary>
    /// Sets a GameObject's Valid to false on when a AnimationComponent .IsFinished() equals true.
    /// </summary>
    class InvalidatesWhenAnimationIsFinishedComponent : Component
    {
        public InvalidatesWhenAnimationIsFinishedComponent(GameObject parent) : base(parent)
        {  
        }

        public override void HandleMessage(Message message)
        {
            base.HandleMessage(message);
            if(message is AnimationFinishedMessage)
            {
                Parent.HandleMessage(new InvalidateMessage(this));
            }
        }
    }
}
