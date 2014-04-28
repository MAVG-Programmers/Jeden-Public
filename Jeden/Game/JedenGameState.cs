using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine;
using Jeden.Game.Physics;
using Jeden.Engine.Object;
using Jeden.Engine.Render;
using SFML.Graphics;

namespace Jeden.Game
{
    //Main play state
    /// <summary>
    /// The ingame/play state of the game.
    /// </summary>
    class JedenGameState : GameState
    {
        public PhysicsManager PhysicsMgr;

        /// <summary>
        /// A new instance of JedenGameState.
        /// </summary>
        public JedenGameState()
        {
            ControlMap = new JedenPlayerInput();
            PhysicsMgr = new PhysicsManager();
            GenTestLevel();
        }

        public void GenTestLevel()
        {
            
            GameObjects.Add(new Player(this));
            RenderMgr.Camera.Target = GameObjects[0];


            GameObject go = new GameObject(this);
            SFML.Graphics.Texture texture = new SFML.Graphics.Texture("assets/parallax0.png");

            ParallaxRenderComponent prc = RenderMgr.MakeNewParallaxComponent(go, texture, 0.5f);
            prc.ZIndex = 2;
            prc.ParallaxFactor = 0.5f;
            go.AddComponent<ParallaxRenderComponent>(prc);



            go = new GameObject(this);
            texture = new SFML.Graphics.Texture("assets/parallax2.png");

            prc = RenderMgr.MakeNewParallaxComponent(go, texture, 0.5f);
            prc.ZIndex = 0;
            prc.ParallaxFactor = 0.05f;
            go.AddComponent<ParallaxRenderComponent>(prc);

            go = new GameObject(this);
            texture = new SFML.Graphics.Texture("assets/parallax1.png");

            prc = RenderMgr.MakeNewParallaxComponent(go, texture, 0.5f);
            prc.ZIndex = 1;
            prc.ParallaxFactor = 0.2f;
            go.AddComponent<ParallaxRenderComponent>(prc);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PhysicsMgr.Update(gameTime);
            
            //Draw frame last
            RenderMgr.Update(gameTime);
        }

    }
}
