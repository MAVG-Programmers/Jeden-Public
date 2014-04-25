using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine;

namespace Jeden.Game.Physics
{
    class PhysicsManager
    {
        //TODO: integrate Farseer physics
        List<PhysicsComponent> Components;

        public PhysicsManager()
        {
            Components = new List<PhysicsComponent>();
        }

        //Update tick all components owned by this manager
        public void Update(GameTime gameTime)
        {
            foreach (var comp in Components)
            {
                comp.Update(gameTime);
            }
        }
    }
}
