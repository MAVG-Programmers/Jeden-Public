using System;
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
        /// <summary>
        /// The amount of Health that is reduced
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// The GameObject that created the attack
        /// </summary>
        public GameObject Attacker {get; set;}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attacker">The GameObject that created the Attack</param>
        /// <param name="parent">The GameObject that holds the component</param>
        public AttackComponent(GameObject attacker, GameObject parent) : base(parent)
        {
            Attacker = attacker;
            Damage = 65;
        }
    }
}
