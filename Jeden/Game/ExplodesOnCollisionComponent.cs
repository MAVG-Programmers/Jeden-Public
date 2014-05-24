
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Game.Physics;
using SFML.Window;

namespace Jeden.Game
{
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
                CollisionMessage collisionMessage = message as CollisionMessage;
                GameObjectFactory.CreateExplosion(Parent.Position);
                // suicide
                Parent.HandleMessage(new InvalidateMessage(this));
            }
        }
    }
}
