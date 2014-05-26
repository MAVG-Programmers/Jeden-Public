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
        public MeleeWeaponComponent MeleeWeaponComponent { get; set; }
        public GunWeaponComponent GunWeaponComponent { get; set; }
        public WeaponHoldingComponent(MeleeWeaponComponent meleeWeaponComponent, GunWeaponComponent gunWeaponComponent, GameObject parent)
            : base(parent)
        {
            MeleeWeaponComponent = meleeWeaponComponent;
            GunWeaponComponent = gunWeaponComponent;
        }
    }
}
