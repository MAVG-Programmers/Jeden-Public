
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
        public static GameObject CreatePlayer(Vector2f position)
        {
            GameObject player = new GameObject(GameState);
            player.Position = position;

            Texture texture = new Texture("assets/player.png");
            Texture texture2 = new Texture("assets/test.png");

            AnimationRenderComponent arc = RenderMgr.MakeNewAnimationComponent(player);

            arc.AddFrame(texture);
            //arc.AddFrame(texture2);
            arc.WorldWidth = texture.Size.X;
            arc.WorldHeight = texture.Size.Y;
            arc.ZIndex = 1000;
            arc.FrameTime = 1.0f;

            player.AddComponent<RenderComponent>(arc);
            player.AddComponent<PhysicsComponent>(PhysicsMgr.MakeNewComponent(player, 64, 128, true));

            player.AddComponent(new CharacterControllerComponent(player));

            GameState.GameObjects.Add(player);

            return player;
        }


        static Texture BulletTexture = new Texture("assets/bullet.png");
        public static GameObject CreateBullet(GameObject attacker, Vector2f position, Vector2f direction)
        {
            float SPEED = 1000.0f;

            GameObject gameObject = new GameObject(GameState);
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, 40, 40, true);
            physicsComp.BoundingBox.LinearVelocity = new Microsoft.Xna.Framework.Vector2(direction.X, direction.Y) * SPEED;
            gameObject.AddComponent(physicsComp);


            AttackComponent attackComp = new AttackComponent(attacker, gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, BulletTexture);
            renderComp.WorldWidth = 40;
            renderComp.WorldHeight = 40;
            renderComp.ZIndex = 100;
            gameObject.AddComponent(renderComp);

            gameObject.AddComponent(new ExplodesOnCollisionComponent(gameObject));

            return gameObject;
        }

        static Texture EnemyTexture = new Texture("assets/player.png");
        public static GameObject CreateEnemy(Vector2f position)
        {

            GameObject enemy = new GameObject(GameState);
            GameState.GameObjects.Add(enemy);
            enemy.Position = position;
            AnimationRenderComponent arc = RenderMgr.MakeNewAnimationComponent(enemy);

            arc.AddFrame(EnemyTexture);
            arc.WorldWidth = EnemyTexture.Size.X;
            arc.WorldHeight = EnemyTexture.Size.Y;
            arc.ZIndex = 1000;
            arc.FrameTime = 1.0f;

            enemy.AddComponent<RenderComponent>(arc);
            enemy.AddComponent(new HealthComponent(enemy, 100, 1));

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(enemy, 54, 128, true);
            enemy.AddComponent<PhysicsComponent>(physicsComp);

            CharacterControllerComponent charControllerComp = new CharacterControllerComponent(enemy);
            enemy.AddComponent(charControllerComp);

            return enemy;
        }

        public static RenderManager RenderMgr;
        public static PhysicsManager PhysicsMgr;
        public static JedenGameState GameState;
    }
}
