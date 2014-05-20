
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
using Jeden.Engine;

namespace Jeden.Game
{
    class GameObjectFactory
    {

        static ConfigFileParser playerConfigFileParser = new ConfigFileParser("cfg/player.txt");
        static ConfigFileParser bulletConfigFileParser = new ConfigFileParser("cfg/bullet.txt");
        /// <summary>
        /// Create's the main player
        /// </summary>
        /// <param name="position">The starting position of the player</param>
        /// <returns></returns>
        public static GameObject CreatePlayer(Vector2f position)
        {
            GameObject player = new GameObject();
            player.Position = position;

            AnimationSetRenderComponent arc = RenderMgr.MakeNewAnimationSetComponent(player);

            arc.AddAnimation("Walking", "cfg/player_walking_anim.txt");
            arc.AddAnimation("Attacking", "cfg/player_attacking_anim.txt");
            arc.SetIsLooping("Attacking", false);

            arc.SetAnimation("Walking");
            arc.ZIndex = playerConfigFileParser.GetAsInt("ZIndex");

            arc.WorldWidth = playerConfigFileParser.GetAsFloat("SpriteWidth");
            arc.WorldHeight = playerConfigFileParser.GetAsFloat("SpriteHeight");
            player.AddComponent(arc);

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(
                player,
                playerConfigFileParser.GetAsFloat("CollisionWidth"),
                playerConfigFileParser.GetAsFloat("CollisionHeight"), 
                PhysicsManager.PlayerCategory, 
                PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, 
                true);


            physicsComp.Body.Friction = playerConfigFileParser.GetAsFloat("Friction");
            physicsComp.Body.GravityScale = playerConfigFileParser.GetAsFloat("GravityScale");
            physicsComp.Body.LinearDamping = playerConfigFileParser.GetAsFloat("LinearDamping");
            physicsComp.Body.Mass = playerConfigFileParser.GetAsFloat("Mass");
            physicsComp.Body.Restitution = playerConfigFileParser.GetAsFloat("Restitution");


            player.AddComponent(physicsComp);

            CharacterControllerComponent controller = new CharacterControllerComponent(arc, physicsComp, player);
            controller.WalkImpulse = playerConfigFileParser.GetAsFloat("WalkImpulse");
            controller.JumpImpulse = playerConfigFileParser.GetAsFloat("JumpImpulse");

            player.AddComponent(controller);

            player.AddComponent(new HealthComponent(player, playerConfigFileParser.GetAsFloat("MaxHealth")));

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

            GameObject gameObject = new GameObject();
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, 
                bulletConfigFileParser.GetAsFloat("CollisionWidth"), 
                bulletConfigFileParser.GetAsFloat("CollisionHeight"),
                PhysicsManager.PlayerCategory, PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, true);
            physicsComp.Body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(direction.X, direction.Y) * bulletConfigFileParser.GetAsFloat("Speed");
            physicsComp.Body.GravityScale = bulletConfigFileParser.GetAsFloat("GravityScale");
            gameObject.AddComponent(physicsComp);


            AttackComponent attackComp = new AttackComponent(attacker, gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, BulletTexture);
            renderComp.WorldWidth = bulletConfigFileParser.GetAsFloat("SpriteWidth");
            renderComp.WorldHeight = bulletConfigFileParser.GetAsFloat("SpriteHeight");
            renderComp.ZIndex = bulletConfigFileParser.GetAsInt("ZIndex");
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
            GameObject gameObject = new GameObject();
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position + new Vector2f(length * 0.5f * xdirection, 0);

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, length, width,  
            PhysicsManager.PlayerCategory, PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, true);
            physicsComp.Body.GravityScale = 0.0f;
            gameObject.AddComponent(physicsComp);

            AttackComponent attackComp = new AttackComponent(attacker, gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, SwordTexture);
            renderComp.LocalPosition = position + new Vector2f(length * 0.5f * xdirection, 0);
            renderComp.WorldWidth = length;
            renderComp.WorldHeight = width;
            renderComp.ZIndex = 100;
            gameObject.AddComponent(renderComp);

            FrameLifetimeComponent frameLifetimeComponent = new FrameLifetimeComponent(1, gameObject);
            gameObject.AddComponent(frameLifetimeComponent);

            return gameObject;
        }

        static Texture StingerTexture = new Texture("assets/stinger.png");
        /// <summary>
        /// Creates a new Bullet
        /// </summary>
        /// <param name="attacker">The GameObject that fired the bullet</param>
        /// <param name="position">The starting position of the bullet</param>
        /// <param name="direction">The direction in which the bullet was fired[needs to be unit length]</param>
        /// <returns></returns>
        public static GameObject CreateStinger(GameObject attacker, Vector2f position, Vector2f direction)
        {
            float SPEED = 1000.0f;

            GameObject gameObject = new GameObject();
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, 1, 1,
                PhysicsManager.EnemyCategory, PhysicsManager.PlayerCategory | PhysicsManager.MapCategory, true);
            physicsComp.Body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(direction.X, direction.Y) * SPEED;
            physicsComp.Body.GravityScale = 0.0f;
            physicsComp.Body.LinearDamping = 0.0f;
            physicsComp.Body.IgnoreGravity = true;
            gameObject.AddComponent(physicsComp);


            AttackComponent attackComp = new AttackComponent(attacker, gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, StingerTexture);
            renderComp.WorldWidth = 1;
            renderComp.WorldHeight = 1;
            renderComp.ZIndex = 100;
            gameObject.AddComponent(renderComp);

            gameObject.AddComponent(new ExplodesOnCollisionComponent(gameObject));

            return gameObject;
        }
        
        static Texture EnemyTexture = new Texture("assets/bee.png");
        /// <summary>
        /// Create's an enemy
        /// </summary>
        /// <param name="position">the starting position of the enemy</param>
        /// <returns></returns>
        public static GameObject CreateEnemy(Vector2f position)
        {

            GameObject enemy = new GameObject();
            GameState.GameObjects.Add(enemy);
            enemy.Position = position;
            AnimationSetRenderComponent arc = RenderMgr.MakeNewAnimationSetComponent(enemy);

            arc.AddFrame("Flying", EnemyTexture);
            arc.WorldWidth = 1;
            arc.WorldHeight = 1;
            arc.ZIndex = 1000;
            arc.SetFrameTime("Flying", 2.0f);
            arc.SetAnimation("Flying");
            enemy.AddComponent(arc);

            enemy.AddComponent(new HealthComponent(enemy, 100));

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(enemy, 1, 1, PhysicsManager.EnemyCategory, PhysicsManager.PlayerCategory | PhysicsManager.MapCategory, true);
            enemy.AddComponent(physicsComp);

            FlyingBugControllerComponent charControllerComp = new FlyingBugControllerComponent(arc, physicsComp, enemy);
            enemy.AddComponent(charControllerComp);

            FlyingBugAIComponent flyingBugAIComponent = new FlyingBugAIComponent(enemy.Position, enemy);
            enemy.AddComponent(flyingBugAIComponent);

            return enemy;
        }


        static Texture ExplosionTexture1 = new Texture("assets/explosion.png");
        static public GameObject CreateExplosion(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent(gameObject);
            animationRenderComponent.FrameTime = 0.1f;

            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                {
                    animationRenderComponent.AddFrame(ExplosionTexture1, new IntRect(x * 64, y * 64, 64, 64));
                }

            animationRenderComponent.WorldWidth = 1;
            animationRenderComponent.WorldHeight = 1;
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = 175;
            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }

        public static GameObject CreateDeadGuy(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent("cfg/player_death_anim.txt", gameObject);

            animationRenderComponent.WorldWidth = 1;
            animationRenderComponent.WorldHeight = 2;
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = 175;
            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }

        public static GameObject CreateDeadBug(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent("cfg/flying_bug_death_anim.txt", gameObject);

            animationRenderComponent.WorldWidth = 1;
            animationRenderComponent.WorldHeight = 1;
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = 175;

            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }

        public static GameObject CreateShieldDamgageFeedback(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent("cfg/shield_anim.txt", gameObject);

            animationRenderComponent.WorldWidth = 1;
            animationRenderComponent.WorldHeight = 2;
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = 1775;
            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }


        public static RenderManager RenderMgr;
        public static PhysicsManager PhysicsMgr;
        public static JedenGameState GameState;
    }
}
