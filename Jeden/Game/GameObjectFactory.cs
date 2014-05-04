
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jeden.Engine.Render;
using Jeden.Game.Physics;
using SFML.Window;
using SFML.Graphics;
using Jeden.Engine.Object;

namespace Jeden.Game
{
    class GameObjectFactory
    {
        public static Player CreatePlayer(Vector2f position)
        {
            Player player = new Player(GameState);

            Texture texture = new Texture("assets/player.png");
            Texture texture2 = new Texture("assets/test.png");

            AnimationRenderComponent arc = RenderMgr.MakeNewAnimationComponent(player);

            arc.AddFrame(texture);
            arc.AddFrame(texture2);
            arc.WorldWidth = 64;
            arc.WorldHeight = 128;
            arc.ZIndex = 1000;
            arc.FrameTime = 1.0f;

            player.AddComponent<RenderComponent>(arc);
            player.AddComponent<PhysicsComponent>(PhysicsMgr.MakeNewComponent(player, 64, 128, true));

            player.AddComponent(new CharacterControllerComponent(player));

            GameState.GameObjects.Add(player);

            return player;
        }


        static Texture BulletTexture = new Texture("assets/test.png");
        public static GameObject CreateBullet(Vector2f position, Vector2f direction)
        {
            float SPEED = 1000.0f;

            GameObject gameObject = new GameObject(GameState);
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, 10, 10, true);
            physicsComp.BoundingBox.LinearVelocity = new Microsoft.Xna.Framework.Vector2(direction.X, direction.Y) * SPEED;
            gameObject.AddComponent(physicsComp);


            AttackComponent attackComp = new AttackComponent(gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, BulletTexture);
            renderComp.WorldWidth = 10;
            renderComp.WorldHeight = 10;
            renderComp.ZIndex = 100;
            gameObject.AddComponent(renderComp);

            return gameObject;
        }

        static Texture EnemyTexture = new Texture("assets/player.png");
        public static GameObject CreateEnemy(Vector2f position)
        {

            GameObject enemy = new GameObject(GameState);
            GameState.GameObjects.Add(enemy);
            enemy.Position = position;
            RenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(enemy, EnemyTexture);
            renderComp.ZIndex = 30;
            enemy.AddComponent(renderComp);
            enemy.AddComponent(new HealthComponent(enemy, 100, 1));

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(enemy, 54, 128, true);
            enemy.AddComponent<PhysicsComponent>(physicsComp);

            return enemy;
        }

        public static RenderManager RenderMgr;
        public static PhysicsManager PhysicsMgr;
        public static JedenGameState GameState;
    }
}
