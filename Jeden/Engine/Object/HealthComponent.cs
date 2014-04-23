using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Object
{
    class HealthComponent : Component
    {
        public int Health { get; set; }

        public HealthComponent(GameObject parent, int hp) : base(parent)
        {
            Health = hp;
        }

        public void Damage(int amount)
        {
            //TODO: Calculate resistances to damage taken
            Health -= amount;
        }

        public override void Update(int dTime)
        {
            if (Health <= 0)
            {
                //TODO: Throw event for death
            }
        }
    }
}
