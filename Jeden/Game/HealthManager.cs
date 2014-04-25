using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Engine;

namespace Jeden.Game
{
    class HealthManager
    {
        private List<HealthComponent> Components;

        public HealthManager()
        {
            Components = new List<HealthComponent>();
        }

        //Update tick all components owned by this manager
        public void Update(GameTime gameTime)
        {
            foreach (var comp in Components)
            {
                comp.Update(gameTime);
            }
        }

        //Creates a render component for a GameObject to use
        //Also adds the new component to the list of those in use
        public HealthComponent MakeNewComponent(GameObject owner, int health)
        {
            HealthComponent component = new HealthComponent(owner, health);
            Components.Add(component);
            return component;
        }
    }
}
