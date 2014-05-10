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
            ControlMap = new JedenPlayerInput(); // TODO: not setting the InputMap
            PhysicsMgr = new PhysicsManager();
            GameObjectFactory.PhysicsMgr = PhysicsMgr;
            GameObjectFactory.GameState = this;

            GenTestLevel();
            
        }

        public void GenTestLevel()
        {
            // Just making a bunch of random stuff for now...

            GameObjectFactory.RenderMgr = RenderMgr; // TODO: put somewhere better

            player = GameObjectFactory.CreatePlayer(new Vector2f(100, 0));
            RenderMgr.Camera.Target = player;

            weapon = new GameObject(this);
            GunWeaponComponent weaponComp = new GunWeaponComponent(player, weapon);
            weapon.AddComponent(weaponComp);
            GameObjects.Add(weapon);
            weaponComp.AttackDelay = 1.0f;

            player.AddComponent(new WeaponHoldingComponent(weaponComp, player));


            TileMap tileMap = new TileMap("assets/testmap.tmx");

            GameObject tileMapGo = new GameObject(this);
            GameObjects.Add(tileMapGo);
            TileMapRenderComponent tmrc = RenderMgr.MakeNewTileMapComponent(tileMapGo);
            tileMap.SetRenderComponent(tmrc);
            tileMapGo.AddComponent<RenderComponent>(tmrc);
            tmrc.ZIndex = 40;


            GameObject go;

            foreach (TileMap.ParallaxSprite sprite in tileMap.ParallaxSprites)
            {
                go = new GameObject(this);
                ParallaxRenderComponent prc;
                go.Position = sprite.Position;
                prc = RenderMgr.MakeNewParallaxComponent(go, sprite.Texture, sprite.ParallaxFactor);
                prc.WorldWidth = sprite.Width;
                prc.WorldHeight = sprite.Height;
                prc.ZIndex = sprite.ZIndex;
                go.AddComponent<RenderComponent>(prc);
                GameObjects.Add(go);

            }

            foreach (TileMap.PhysicsObject pobj in tileMap.PhysicsObjects)
            {

                Vector2f Position = new Vector2f(pobj.Position.X, pobj.Position.Y);

                go = new GameObject(this);
                go.Position = pobj.Position;
                PhysicsComponent pc = PhysicsMgr.MakeNewComponent(
                    go, pobj.Width, pobj.Height,
                    PhysicsManager.MapCategory,
                    PhysicsManager.PlayerCategory | PhysicsManager.EnemyCategory,
                    false);
                GameObjects.Add(go);
            }

            for (int i = 0; i < 10; i++)
                GameObjectFactory.CreateEnemy(new Vector2f(i * 100 + 500, 100));

            GameObject cloth = new GameObject(this);
            GameObjects.Add(cloth);
            BandanaRenderComponent crc = new BandanaRenderComponent(player, cloth);
            crc.ZIndex = 150;
            cloth.AddComponent<RenderComponent>(crc);
            RenderMgr.Components.Add(crc);

            Music = new SFML.Audio.Music("assets/Widzy.wav");
       
            Music.Play();
        }



        void RemoveGameObject(GameObject obj)
        {
            // should components hold an instance of their manager so that in situations like this we can simply do
            // Manager m = comp.Manager;
            // m.RemoveComponenct(comp);
            // ??
            /*
            foreach(Component comp in obj.Components.Values)
            {
                if(comp is RenderComponent)
                    RenderMgr.RemoveComponent(comp as RenderComponent);

                if (comp is PhysicsComponent)
                    PhysicsMgr.RemoveComponent(comp as PhysicsComponent);
            }

            GameObjects.Remove(obj);
             */
            obj.Valid = false;
        }

        void InputHack()
        {
            //Temp hack until CommandMap is implemented
            if (ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Left))
            {
                player.HandleMessage(new WalkLeftMessage(null));
                weapon.HandleMessage(new WalkLeftMessage(null));
            }
            if (ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Right))
            {
                player.HandleMessage(new WalkRightMessage(null));
                weapon.HandleMessage(new WalkRightMessage(null));
            }
            if (ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Up))
            {
                player.HandleMessage(new JumpMessage(null));
                weapon.HandleMessage(new JumpMessage(null));
            }
            if(ControlMap.InputMgr.IsKeyDown(Keyboard.Key.Space))
            {
                player.HandleMessage(new AttackMessage(null));
              //  weapon.HandleMessage(new AttackMessage(null));
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
                    
                    foreach (Component comp in temp.Components.Values)
                    {
                        if (comp is RenderComponent)
                            RenderMgr.RemoveComponent(comp as RenderComponent);

                        if (comp is PhysicsComponent)
                            PhysicsMgr.RemoveComponent(comp as PhysicsComponent);
                    }
                }
            }

            GameObjects.RemoveRange(GameObjects.Count - removed, removed);
        }

        public override void Update(GameTime gameTime)
        {
            PhysicsMgr.Update(gameTime);
            InputHack();
            base.Update(gameTime);

            //Draw frame last
            RenderMgr.Update(gameTime);

            RemoveInvalidGameObjects();
        }

    }
}
