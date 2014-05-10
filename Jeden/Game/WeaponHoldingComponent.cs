using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;

namespace Jeden.Game
{
    class WeaponHoldingComponent : Component
    {
        public WeaponComponent WeaponComponent { get; set; } 

        public WeaponHoldingComponent(WeaponComponent weaponComponent, GameObject parent) : base(parent)
        {
            WeaponComponent = weaponComponent;
        }
    }
}
