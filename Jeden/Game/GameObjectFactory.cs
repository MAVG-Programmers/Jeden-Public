
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
using FarseerPhysics.Dynamics;

namespace Jeden.Game
{
    class GameObjectFactory
    {

        static ConfigFileParser playerConfigFileParser = new ConfigFileParser("cfg/player.txt");
        static ConfigFileParser flyingBugConfigFileParser = new ConfigFileParser("cfg/flying_bug.txt");
        static ConfigFileParser bulletConfigFileParser = new ConfigFileParser("cfg/bullet.txt");
        static ConfigFileParser jabConfigFileParser = new ConfigFileParser("cfg/jab.txt");
        static ConfigFileParser explosionConfigFileParser = new ConfigFileParser("cfg/explosion.txt");
        static ConfigFileParser stingerConfigFileParser = new ConfigFileParser("cfg/stinger.txt");
        static ConfigFileParser deadFlyingBugConfigFileParser = new ConfigFileParser("cfg/dead_flying_bug.txt");
        static ConfigFileParser deadPlayerConfigFileParser = new ConfigFileParser("cfg/dead_player.txt");
        static ConfigFileParser shieldDamageEffectConfigFileParser = new ConfigFileParser("cfg/shield_damage_effect.txt");
        static ConfigFileParser meleeWeaponConfigFileParser = new ConfigFileParser("cfg/melee_weapon.txt");
        static ConfigFileParser gunWeaponConfigFileParser = new ConfigFileParser("cfg/gun_weapon.txt");

        /// <summary>
        /// Create's the main player
        /// </summary>
        /// <param name="position">The starting position of the player</param>
        /// <returns></returns>
        public static GameObject CreatePlayer(Vector2f position)
        {
            GameObject player = new GameObject();
            player.Position = position;

            AnimationSetRenderComponent anims = RenderMgr.MakeNewAnimationSetComponent(player);

            anims.AddAnimation("Walking", "cfg/player_walking_anim.txt");
            anims.AddAnimation("Attacking", "cfg/player_melee_attacking_anim.txt");
            anims.AddAnimation("Idle", "cfg/player_idle_anim.txt");
            anims.AddAnimation("Jumping", "cfg/player_jumping_anim.txt");
            anims.AddAnimation("Falling", "cfg/player_falling_anim.txt");
            //idle
            //jumping
            //ducking
            anims.SetIsLooping("Attacking", false);
            anims.SetIsLooping("Jumping", false);

            anims.SetAnimation("Walking");
            anims.ZIndex = playerConfigFileParser.GetAsInt("ZIndex");

            anims.WorldWidth = playerConfigFileParser.GetAsFloat("SpriteWidth");
            anims.WorldHeight = playerConfigFileParser.GetAsFloat("SpriteHeight");
            player.AddComponent(anims);

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(
                player,
                playerConfigFileParser.GetAsFloat("CollisionWidth"),
                playerConfigFileParser.GetAsFloat("CollisionHeight"), 
                PhysicsManager.PlayerCategory, 
                PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, 
                BodyType.Dynamic);


            physicsComp.Body.Friction = playerConfigFileParser.GetAsFloat("Friction");
            physicsComp.Body.GravityScale = playerConfigFileParser.GetAsFloat("GravityScale");
            physicsComp.Body.LinearDamping = playerConfigFileParser.GetAsFloat("LinearDamping");
            physicsComp.Body.Mass = playerConfigFileParser.GetAsFloat("Mass");
            physicsComp.Body.Restitution = playerConfigFileParser.GetAsFloat("Restitution");

            player.AddComponent(physicsComp);

            CharacterControllerComponent controller = new CharacterControllerComponent(anims, physicsComp, player);
            controller.WalkImpulse = playerConfigFileParser.GetAsFloat("WalkImpulse");
            controller.JumpImpulse = playerConfigFileParser.GetAsFloat("JumpImpulse");

            player.AddComponent(controller);

            player.AddComponent(new HealthComponent(player, playerConfigFileParser.GetAsFloat("MaxHealth")));

            GameState.GameObjects.Add(player);

            return player;
        }


        /// <summary>
        /// Creates a new Bullet
        /// </summary>
        /// <param name="attacker">The GameObject that fired the bullet</param>
        /// <param name="position">The starting position of the bullet</param>
        /// <param name="direction">The direction in which the bullet was fired[needs to be unit length]</param>
        /// <returns></returns>
        public static GameObject CreateBullet(GameObject attacker, Vector2f position, int direction)
        {

            GameObject gameObject = new GameObject();
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, 
                bulletConfigFileParser.GetAsFloat("CollisionWidth"), 
                bulletConfigFileParser.GetAsFloat("CollisionHeight"),
                PhysicsManager.PlayerCategory, PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, BodyType.Kinematic);
            physicsComp.Body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(direction, 0) * bulletConfigFileParser.GetAsFloat("Speed");
            physicsComp.Body.GravityScale = bulletConfigFileParser.GetAsFloat("GravityScale");
            gameObject.AddComponent(physicsComp);


            AttackComponent attackComp = new AttackComponent(attacker, bulletConfigFileParser.GetAsFloat("Damage"), gameObject);
            gameObject.AddComponent(attackComp);

            AnimationRenderComponent renderComp = RenderMgr.MakeNewAnimationComponent("cfg/bullet_anim.txt", gameObject);
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
        public static GameObject CreateJab(GameObject attacker, MeleeWeaponComponent meleeWeapon, Vector2f position, int xdirection)
        {
            float width = jabConfigFileParser.GetAsFloat("CollisionWidth");
            float height = jabConfigFileParser.GetAsFloat("CollisionHeight");
            float damage = jabConfigFileParser.GetAsFloat("Damage");
            float forwardX = jabConfigFileParser.GetAsFloat("ForwardX");
            float backwardX = jabConfigFileParser.GetAsFloat("BackwardX");
            float forwardTime = jabConfigFileParser.GetAsFloat("ForwardTime");
            float backwardTime = jabConfigFileParser.GetAsFloat("BackwardTime");

            GameObject gameObject = new GameObject();
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(gameObject, width, height,  
            PhysicsManager.PlayerCategory, PhysicsManager.EnemyCategory | PhysicsManager.MapCategory, BodyType.Kinematic);
            physicsComp.Body.GravityScale = 0.0f;
            physicsComp.Body.IsSensor = true;
            
            gameObject.AddComponent(physicsComp);

            AttackComponent attackComp = new AttackComponent(attacker, damage, gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, SwordTexture);
            renderComp.LocalPosition = position;
            renderComp.WorldWidth = width;
            renderComp.WorldHeight = height;
            renderComp.ZIndex = 100;
            gameObject.AddComponent(renderComp);

            
            JabControllerComponent scc = new JabControllerComponent(physicsComp, meleeWeapon, 
                new Vector2f(forwardX * xdirection, 0), 
                new Vector2f(backwardX * xdirection, 0), 
                forwardTime, 
                backwardTime, 
                gameObject);
            
            gameObject.AddComponent(scc);

            //FrameLifetimeComponent frameLifetimeComponent = new FrameLifetimeComponent(16, gameObject);
            //gameObject.AddComponent(frameLifetimeComponent);

            return gameObject;
        }

        public static GameObject CreateMeleeWeapon(GameObject player)
        {
            GameObject gameObject = new GameObject();
            Vector2f offset = new Vector2f(meleeWeaponConfigFileParser.GetAsFloat("OffsetX"), meleeWeaponConfigFileParser.GetAsFloat("OffsetY"));
            MeleeWeaponComponent meleeWeaponComponent = new MeleeWeaponComponent(player, offset, gameObject);
            gameObject.AddComponent(meleeWeaponComponent);
            GameState.GameObjects.Add(gameObject);

            meleeWeaponComponent.AttackDelay = meleeWeaponConfigFileParser.GetAsFloat("AttackDelay");
            return gameObject;
        }

        public static GameObject CreateGunWeapon(GameObject player)
        {
            GameObject gameObject = new GameObject();

            Vector2f offset = new Vector2f(gunWeaponConfigFileParser.GetAsFloat("OffsetX"), gunWeaponConfigFileParser.GetAsFloat("OffsetY"));
            GunWeaponComponent gunWeaponComponent = new GunWeaponComponent(player, offset, gameObject);
            gameObject.AddComponent(gunWeaponComponent);
            GameState.GameObjects.Add(gameObject);
            gunWeaponComponent.AttackDelay = gunWeaponConfigFileParser.GetAsFloat("AttackDelay");

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

            
            GameObject gameObject = new GameObject();
            GameState.GameObjects.Add(gameObject);
            gameObject.Position = position;

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(
                gameObject, 
                stingerConfigFileParser.GetAsFloat("CollisionWidth"),
                stingerConfigFileParser.GetAsFloat("CollisionHeight"),
                PhysicsManager.EnemyCategory, 
                PhysicsManager.PlayerCategory | PhysicsManager.MapCategory, 
                BodyType.Kinematic);
            physicsComp.Body.LinearVelocity = 
                new Microsoft.Xna.Framework.Vector2(direction.X, direction.Y) * stingerConfigFileParser.GetAsFloat("Speed");
            physicsComp.Body.LinearDamping = 0.0f;
            physicsComp.Body.IgnoreGravity = true;
            gameObject.AddComponent(physicsComp);


            AttackComponent attackComp = new AttackComponent(attacker, stingerConfigFileParser.GetAsFloat("Damage"), gameObject);
            gameObject.AddComponent(attackComp);

            SpriteRenderComponent renderComp = RenderMgr.MakeNewSpriteComponent(gameObject, StingerTexture);
            renderComp.WorldWidth = stingerConfigFileParser.GetAsFloat("SpriteWidth");
            renderComp.WorldHeight = stingerConfigFileParser.GetAsFloat("SpriteHeight");
            renderComp.ZIndex = stingerConfigFileParser.GetAsInt("ZIndex");
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
        public static GameObject CreateFlyingBug(Vector2f position)
        {

            GameObject enemy = new GameObject();
            GameState.GameObjects.Add(enemy);
            enemy.Position = position;
            AnimationSetRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationSetComponent(enemy);

            animationRenderComponent.AddFrame("Flying", EnemyTexture);
            animationRenderComponent.WorldWidth = flyingBugConfigFileParser.GetAsFloat("SpriteWidth");
            animationRenderComponent.WorldHeight = flyingBugConfigFileParser.GetAsFloat("SpriteHeight");
            animationRenderComponent.ZIndex = flyingBugConfigFileParser.GetAsInt("ZIndex");
            
            animationRenderComponent.SetFrameTime("Flying", 2.0f);
            animationRenderComponent.SetAnimation("Flying");
            enemy.AddComponent(animationRenderComponent);

            enemy.AddComponent(new HealthComponent(enemy, flyingBugConfigFileParser.GetAsFloat("Health")));

            PhysicsComponent physicsComp = PhysicsMgr.MakeNewComponent(enemy, 
                flyingBugConfigFileParser.GetAsFloat("CollisionWidth"),
                flyingBugConfigFileParser.GetAsFloat("CollisionHeight"), 
                PhysicsManager.EnemyCategory, PhysicsManager.PlayerCategory | PhysicsManager.MapCategory, 
                BodyType.Dynamic);
            physicsComp.Body.Mass = flyingBugConfigFileParser.GetAsFloat("Mass");
            enemy.AddComponent(physicsComp);

            FlyingBugControllerComponent charControllerComp = new FlyingBugControllerComponent(
                animationRenderComponent, 
                physicsComp, 
                flyingBugConfigFileParser.GetAsFloat("MovementImpulse"),
                enemy);
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

            animationRenderComponent.WorldWidth = explosionConfigFileParser.GetAsFloat("SpriteWidth");
            animationRenderComponent.WorldHeight = explosionConfigFileParser.GetAsFloat("SpriteHeight");
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = explosionConfigFileParser.GetAsInt("ZIndex");
            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }

        public static GameObject CreateDeadPlayer(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent("cfg/player_death_anim.txt", gameObject);

            animationRenderComponent.WorldWidth = deadPlayerConfigFileParser.GetAsFloat("SpriteWidth");
            animationRenderComponent.WorldHeight = deadPlayerConfigFileParser.GetAsFloat("SpriteHeight");
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = deadPlayerConfigFileParser.GetAsInt("ZIndex");
            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }

        public static GameObject CreateDeadFlyingBug(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent("cfg/flying_bug_death_anim.txt", gameObject);

            animationRenderComponent.WorldWidth = deadFlyingBugConfigFileParser.GetAsFloat("SpriteWidth");
            animationRenderComponent.WorldHeight = deadFlyingBugConfigFileParser.GetAsFloat("SpriteHeight");
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = deadFlyingBugConfigFileParser.GetAsInt("ZIndex");

            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }

        public static GameObject CreateShieldDamgageEffect(Vector2f position)
        {
            GameObject gameObject = new GameObject();
            gameObject.Position = position;
            GameState.GameObjects.Add(gameObject);

            AnimationRenderComponent animationRenderComponent = RenderMgr.MakeNewAnimationComponent("cfg/shield_anim.txt", gameObject);

            animationRenderComponent.WorldWidth = shieldDamageEffectConfigFileParser.GetAsFloat("SpriteWidth");
            animationRenderComponent.WorldHeight = shieldDamageEffectConfigFileParser.GetAsFloat("SpriteHeight");
            animationRenderComponent.IsLooping = false;
            animationRenderComponent.ZIndex = shieldDamageEffectConfigFileParser.GetAsInt("ZIndex");
            gameObject.AddComponent(animationRenderComponent);
            gameObject.AddComponent(new InvalidatesWhenAnimationIsFinishedComponent(gameObject));

            return gameObject;
        }


        public static RenderManager RenderMgr;
        public static PhysicsManager PhysicsMgr;
        public static JedenGameState GameState;
    }
}
