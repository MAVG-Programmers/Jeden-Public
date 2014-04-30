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
using SFML.Window;
using Microsoft.Xna.Framework;
using Jeden.Engine.TileMap;

namespace Jeden.Game
{
    //Main play state
    /// <summary>
    /// The ingame/play state of the game.
    /// </summary>
    class JedenGameState : GameState
    {
        Player player;


        public PhysicsManager PhysicsMgr;

        /// <summary>
        /// A new instance of JedenGameState.
        /// </summary>
        public JedenGameState()
        {
            ControlMap = new JedenPlayerInput(); // TODO: not setting the InputMap
            PhysicsMgr = new PhysicsManager();
            GenTestLevel();
        }

        public void GenTestLevel()
        {
            player = new Player(this);
            GameObjects.Add(player);
            RenderMgr.Camera.Target = GameObjects[0];

            TileMap tileMap = new TileMap("assets/testmap.tmx");

            GameObject tileMapGo = new GameObject(this);
            GameObjects.Add(tileMapGo);
            TileMapRenderComponent tmrc = RenderMgr.MakeNewTileMapComponent(tileMapGo);
            tileMap.SetRenderComponent(tmrc);
            tileMapGo.AddComponent<RenderComponent>(tmrc);
            tmrc.ZIndex = 40;


            GameObject go = new GameObject(this);
            SFML.Graphics.Texture texture = new SFML.Graphics.Texture("assets/parallax0.png");

            ParallaxRenderComponent prc = RenderMgr.MakeNewParallaxComponent(go, texture, 0.5f);
            prc.ZIndex = 2;
            prc.ParallaxFactor = 0.5f;
            go.AddComponent<ParallaxRenderComponent>(prc);
            GameObjects.Add(go);

            go = new GameObject(this);
            texture = new SFML.Graphics.Texture("assets/parallax2.png");
            prc = RenderMgr.MakeNewParallaxComponent(go, texture, 0.5f);
            prc.ZIndex = 0;
            prc.ParallaxFactor = 0.05f;
            go.AddComponent<ParallaxRenderComponent>(prc);
            GameObjects.Add(go);

            go = new GameObject(this);
            texture = new SFML.Graphics.Texture("assets/parallax1.png");
            prc = RenderMgr.MakeNewParallaxComponent(go, texture, 0.5f);
            prc.ZIndex = 1;
            prc.ParallaxFactor = 0.2f;
            go.AddComponent<ParallaxRenderComponent>(prc);
            GameObjects.Add(go);

            foreach (TileMap.PhysicsObject pobj in tileMap.PhysicsObjects)
            {

                Vector2f Position = new Vector2f(pobj.Position.X, pobj.Position.Y);

                go = new GameObject(this);
                go.Position = pobj.Position;
                PhysicsComponent pc = PhysicsMgr.MakeNewComponent(go, pobj.Width, pobj.Height, false);
                //SpriteRenderComponent src = RenderMgr.MakeNewSpriteComponent(go, texture);

                //src.ZIndex = 100;
               // go.AddComponent(src);
                GameObjects.Add(go);
            }           

        }

        void InputHack()
        {
            //Temp hack until CommandMap is implemented
            if (ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Left))
            {
                player.GetComponent<PhysicsComponent>().BoundingBox.ApplyLinearImpulse(new Vector2(-1000.0f, 0));
                player.GetComponent<RenderComponent>().FlipX = true;
            }
            if (ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Right))
            {
                player.GetComponent<PhysicsComponent>().BoundingBox.ApplyLinearImpulse(new Vector2(1000.0f, 0));
                player.GetComponent<RenderComponent>().FlipX = false;
            }
            if (ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Up))
            {
                player.GetComponent<PhysicsComponent>().BoundingBox.ApplyLinearImpulse(new Vector2(0.0f, -1000));
            }
        }

        public override void Render(RenderWindow Target)
        {
            RenderMgr.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            InputHack();

            base.Update(gameTime);
            PhysicsMgr.Update(gameTime);


            //Draw frame last
            RenderMgr.Update(gameTime);
        }

    }
}
