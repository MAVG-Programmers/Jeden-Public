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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;


namespace Jeden.Game
{
    //Main play state
    /// <summary>
    /// The ingame/play state of the game.
    /// </summary>
    class JedenGameState : GameState
    {
        GameObject player;
        GameObject weapon;

        public PhysicsManager PhysicsMgr;

        /// <summary>
        /// A new instance of JedenGameState.
        /// </summary>
        public JedenGameState()
        {
            PhysicsMgr = new PhysicsManager();
            GameObjectFactory.PhysicsMgr = PhysicsMgr;
            GameObjectFactory.GameState = this;

            GenTestLevel();
            
        }

        public void GenTestLevel()
        {
            // Just making a bunch of random stuff for now...

            GameObjectFactory.RenderMgr = RenderMgr; // TODO: put somewhere better


            RenderMgr.Camera.Size = new Vector2f(1280 / 64, 720 / 64);

            player = GameObjectFactory.CreatePlayer(new Vector2f(0, -10));
            RenderMgr.Camera.Target = player;

            weapon = new GameObject();
            MeleeWeaponComponent weaponComp = new MeleeWeaponComponent(player, weapon);
            weapon.AddComponent(weaponComp);
            GameObjects.Add(weapon);
            weaponComp.AttackDelay = 1.0f;

            player.AddComponent(new WeaponHoldingComponent(weaponComp, player));


            TileMap tileMap = new TileMap("assets/testmap.tmx", 1.0f/64);

            GameObject tileMapGo = new GameObject();
            GameObjects.Add(tileMapGo);
            TileMapRenderComponent tmrc = RenderMgr.MakeNewTileMapComponent(tileMapGo);
            tileMap.SetRenderComponent(tmrc);
            tileMapGo.AddComponent(tmrc);
            tmrc.ZIndex = 40;


            GameObject go;

            foreach (TileMap.ParallaxSprite sprite in tileMap.ParallaxSprites)
            {
                go = new GameObject();
                ParallaxRenderComponent prc;
                go.Position = sprite.Position;
                prc = RenderMgr.MakeNewParallaxComponent(go, sprite.Texture, sprite.ParallaxFactor);
                prc.WorldWidth = sprite.Width;
                prc.WorldHeight = sprite.Height;
                prc.ZIndex = sprite.ZIndex;
                go.AddComponent(prc);
                GameObjects.Add(go);

            }

            foreach (TileMap.PhysicsObject pobj in tileMap.PhysicsObjects)
            {

                Vector2f Position = new Vector2f(pobj.Position.X, pobj.Position.Y);

                go = new GameObject();
                go.Position = pobj.Position;
                PhysicsComponent pc = PhysicsMgr.MakeNewComponent(
                    go, pobj.Width, pobj.Height,
                    PhysicsManager.MapCategory,
                    PhysicsManager.PlayerCategory | PhysicsManager.EnemyCategory,
                    false);
                GameObjects.Add(go);
            }

            for (int i = 0; i < 2; i++)
                GameObjectFactory.CreateEnemy(new Vector2f(i * 4 + 13, 1));
            

            GameObject cloth = new GameObject();
            GameObjects.Add(cloth);
            BandanaRenderComponent crc = new BandanaRenderComponent(RenderMgr, player, cloth);
            crc.ZIndex = 150;
            cloth.AddComponent(crc);
            RenderMgr.Components.Add(crc);

            Music = new SFML.Audio.Music("assets/Widzy.wav");
       
            Music.Play();
        }

        void InputHack()
        {
            if (InputMgr.IsKeyDown(Keyboard.Key.Left))
            {
                player.HandleMessage(new WalkLeftMessage(null));
                weapon.HandleMessage(new WalkLeftMessage(null));
            }
            if (InputMgr.IsKeyDown(Keyboard.Key.Right))
            {
                player.HandleMessage(new WalkRightMessage(null));
                weapon.HandleMessage(new WalkRightMessage(null));
            }
            if (InputMgr.IsKeyDown(Keyboard.Key.Up))
            {
                player.HandleMessage(new JumpMessage(null));
                weapon.HandleMessage(new JumpMessage(null));
            }
            if(InputMgr.IsKeyDown(Keyboard.Key.Space))
            {
                player.HandleMessage(new AttackMessage(null));
              //  weapon.HandleMessage(new AttackMessage(null));
            }
            if(InputMgr.IsKeyDown(Keyboard.Key.A))
            {
                GameObjectFactory.CreateEnemy(player.Position + new Vector2f(2, 2));
            }
        }

        public override void Render(RenderWindow Target)
        {
            RenderMgr.Draw();
        }

        /// <summary>
        /// Removes all GameObjects with their Valid flag set to false. Also removes all their components from their managers
        /// </summary>
        void RemoveInvalidGameObjects()
        {
            int removed = 0;
            int end = GameObjects.Count;

            for (int i = 0; i < end; i++)
            {
                if (GameObjects[i].Valid == false)
                {
                    GameObject temp = GameObjects[i];
                    GameObjects[i] = GameObjects[end - 1];
                    removed++;
                    end--;
                    
                    foreach (Component comp in temp.Components)
                    {
                        if (comp.Manager != null)
                            comp.Manager.RemoveComponent(comp);
                    }
                }
            }

            GameObjects.RemoveRange(GameObjects.Count - removed, removed);
        }

        public override void Update(GameTime gameTime)
        {
            InputHack();
            base.Update(gameTime);


            PhysicsMgr.Update(gameTime);
      
            //Draw frame last
            RenderMgr.Update(gameTime);

            RemoveInvalidGameObjects();
        }

    }
}
