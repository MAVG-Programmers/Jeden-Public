
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Game.Physics;

namespace Jeden.Game
{
    // No explosions yet...
    class ExplodesOnCollisionComponent : Component
    {
        public ExplodesOnCollisionComponent(GameObject parent) : base(parent)
        {

        }

        public override void HandleMessage(Message message)
        {
            base.HandleMessage(message);
            if(message is CollisionMessage)
            {
                // suicide
                Parent.Valid = false;
            }
        }
    }
}
