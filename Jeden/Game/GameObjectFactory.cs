
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
        /// <summary>
        /// Create's the main player
        /// </summary>
        /// <param name="position">The starting position of the player</param>
        /// <returns></returns>
        public static GameObject CreatePlayer(Vector2f position)
        {
            GameObject player = new GameObject(GameState);
            player.Position = position;


            Texture attack1  = new Texture("assets/attacking_1.png");
            Texture attack2 = new Texture("assets/attacking_2.png");
            Texture attack3 = new Texture("assets/attacking_3.png");
            Texture walking = new Texture("assets/player.png");

            AnimationSetRenderComponent arc = RenderMgr.MakeNewAnimationSetComponent(player);

            arc.AddFrame("Walking", walking);

            arc.AddFrame("Attacking", attack1);
            arc.AddFrame("Attacking", attack2);
            arc.AddFrame("Attacking", attack3);
            arc.WorldWidth = attack1.Size.X;
            arc.WorldHeight = attack1.Size.Y;
            arc.SetFrameTime("Attacking", 0.2f);

            arc.SetIsLooping("Attacking", false);

            arc.SetAnimation("Walking");
            arc.ZIndex = 1000;
            player.AddComponent<RenderComponent>(arc);

            player.AddComponent<PhysicsComponent>(PhysicsMgr.MakeNewComponent(
                player, 64, 128, 
                PhysicsManager.PlayerCategory, 
                PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, 
                true));

            player.AddComponent(new CharacterControllerComponent(player));

            GameState.GameObjects.Add(player);

            return player;
        }


        static Texture BulletTexture = new Texture("assets/bullet.png");
        /// <summary>
        /// Creates a new Bullet
        /// </summary>
        /// <param name="attacker">The GameObject that fired the bullet</param>
        /// <param name="position">The starting position of the bullet</param>
        /// <param name="direction">The direction in which the bullet was fired[needs to be unit length]</param>
        /// <returns></returns>
        public static GameObject CreateBullet(GameObject attacker, Vector2f position, Vector2f direction)
        {
            float SPEED = 1000.0f;

            GameObject gameObject = new GameObject(GameState);
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, 40, 40,
                PhysicsManager.PlayerCategory, PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, true);
            physicsComp.BoundingBox.LinearVelocity = new Microsoft.Xna.Framework.Vector2(direction.X, direction.Y) * SPEED;
            physicsComp.BoundingBox.GravityScale = 0.0f;
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

        static Texture SwordTexture = new Texture("assets/test2.png");
        /// <summary>
        /// Creates a new sword. The sword collision shape is a rectangle at (position.x, position.y-width/2), (position.x, position.y+width/2), (position.x+length, position.y+widht/2), (position.x+length, position.y-width/2).  
        /// </summary>
        /// <param name="attacker">The GameObject that attacked with the sword</param>
        /// <param name="position">The position of the sword</param>
        /// <param name="legth">The length of the sword</param>
        /// <param name="width">The width of the sword</param>
        /// <param name="xdirection">The x direction in which the sword extends(-1, 1).</param>
        /// <returns></returns>
        public static GameObject CreateSword(GameObject attacker, Vector2f position, float length, float width, int xdirection)
        {
            GameObject gameObject = new GameObject(GameState);
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position + new Vector2f(length * 0.5f * xdirection, 0);

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, length, width,  
            PhysicsManager.PlayerCategory, PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, true);
            physicsComp.BoundingBox.GravityScale = 0.0f;
            gameObject.AddComponent(physicsComp);

            AttackComponent attackComp = new AttackComponent(attacker, gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, SwordTexture);
            renderComp.Position = position + new Vector2f(length * 0.5f * xdirection, 0);
            renderComp.WorldWidth = length;
            renderComp.WorldHeight = width;
            renderComp.ZIndex = 100;
            gameObject.AddComponent(renderComp);

            FrameLifetimeComponent frameLifetimeComponent = new FrameLifetimeComponent(1, gameObject);
            gameObject.AddComponent(frameLifetimeComponent);

            return gameObject;
        }

        static Texture EnemyTexture = new Texture("assets/player.png");
        /// <summary>
        /// Create's an enemy
        /// </summary>
        /// <param name="position">the starting position of the enemy</param>
        /// <returns></returns>
        public static GameObject CreateEnemy(Vector2f position)
        {

            GameObject enemy = new GameObject(GameState);
            GameState.GameObjects.Add(enemy);
            enemy.Position = position;
            AnimationSetRenderComponent arc = RenderMgr.MakeNewAnimationSetComponent(enemy);

            arc.AddFrame("Walking", EnemyTexture);
            arc.WorldWidth = EnemyTexture.Size.X;
            arc.WorldHeight = EnemyTexture.Size.Y;
            arc.ZIndex = 1000;
            arc.SetFrameTime("Walking", 1.0f);
            arc.SetAnimation("Walking");
            enemy.AddComponent<RenderComponent>(arc);

            enemy.AddComponent(new HealthComponent(enemy, 100, 1));

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(enemy, 54, 128, PhysicsManager.EnemyCategory, PhysicsManager.PlayerCategory | PhysicsManager.MapCategory, true);
            enemy.AddComponent<PhysicsComponent>(physicsComp);

            CharacterControllerComponent charControllerComp = new CharacterControllerComponent(enemy);
            enemy.AddComponent(charControllerComp);

            return enemy;
        }


        static Texture ExplosionTexture1 = new Texture("assets/explosion.png");
        static public GameObject CreateExplosion(Vector2f position)
        {
            GameObject gameObject = new GameObject(GameState);
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent(gameObject);
            animationRenderComponent.FrameTime = 0.1f;

            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                {
                    animationRenderComponent.AddFrame(ExplosionTexture1, new IntRect(x * 64, y * 64, 64, 64));
                }

            animationRenderComponent.WorldWidth = 64;
            animationRenderComponent.WorldHeight = 64;
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = 175;
            gameObject.AddComponent<RenderComponent>(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(animationRenderComponent, gameObject));

            return gameObject;
        }

        public static Texture DeathTexture1 = new Texture("assets/death1.png");
        public static Texture DeathTexture2 = new Texture("assets/death2.png");
        public static Texture DeathTexture3 = new Texture("assets/death3.png");

        public static GameObject CreateDeadGuy(Vector2f position)
        {
            GameObject gameObject = new GameObject(GameState);
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent(gameObject);
            animationRenderComponent.FrameTime = 0.5f;
            animationRenderComponent.AddFrame(DeathTexture1);
            animationRenderComponent.AddFrame(DeathTexture2);
            animationRenderComponent.AddFrame(DeathTexture3);

            animationRenderComponent.WorldWidth = 200;
            animationRenderComponent.WorldHeight = 142;
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = 175;
            gameObject.AddComponent<RenderComponent>(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(animationRenderComponent, gameObject));

            return gameObject;
        }


        public static RenderManager RenderMgr;
        public static PhysicsManager PhysicsMgr;
        public static JedenGameState GameState;
    }
}
