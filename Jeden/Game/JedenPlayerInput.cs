using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine;
using Jeden.Engine.Object;

namespace Jeden.Game
{




    /// <summary>
    /// The ingame IControlMap.
    /// </summary>
    class JedenPlayerInput : IControlMap
    {
        /// <summary>
        /// The InputManager this IControlMap is watching
        /// </summary>
        public InputManager InputMgr { get; set; }
    }
}
