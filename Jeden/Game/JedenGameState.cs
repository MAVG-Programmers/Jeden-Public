﻿using System;
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
        public PhysicsManager PhysicsMgr;
        public HealthManager HealthMgr;

        /// <summary>
        /// A new instance of JedenGameState.
        /// </summary>
        public JedenGameState()
        {
            ControlMap = new JedenPlayerInput();
            PhysicsMgr = new PhysicsManager();
            HealthMgr = new HealthManager();
            GenTestLevel();
        }

        public void GenTestLevel()
        {
            GameObjects.Add(new Player(this));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HealthMgr.Update(gameTime);
            PhysicsMgr.Update(gameTime);
            
            //Draw frame last
            RenderMgr.Update(gameTime);
        }

    }
}
