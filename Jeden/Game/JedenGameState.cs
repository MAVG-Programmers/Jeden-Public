using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine;
using Jeden.Game.Physics;

namespace Jeden.Game
{
    //Main play state
    /// <summary>
    /// The ingame/play state of the game.
    /// </summary>
    class JedenGameState : GameState
    {
        private PhysicsManager PhysicsMgr;

        /// <summary>
        /// A new instance of JedenGameState.
        /// </summary>
        public JedenGameState()
        {
            ControlMap = new JedenPlayerInput();
        }

    }
}
