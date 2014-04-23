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
    class JedenGameState : GameState
    {
        private PhysicsManager PhysicsMgr;

        public JedenGameState()
        {
            ControlMap = new JedenPlayerInput();
        }

    }
}
