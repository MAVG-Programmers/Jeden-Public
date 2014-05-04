﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;

namespace Jeden.Game
{

    /// <summary>
    /// Represents an Attack spawned from a Weapon component. Could be a bullet, or instace of chainsaw attack or something like that.
    /// </summary>
    class AttackComponent : Component 
    {
        public float Damage { get; set; }

        public AttackComponent(GameObject parent) : base(parent)
        {

            Damage = 100;
        }
    }
}
