using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Game.Physics;
using SFML.Window;
using SFML.Graphics;
using Jeden.Engine.Render;

namespace Jeden.Game
{

    class AttackMessage : Message
    {
        public AttackMessage(Component sender) : base(sender) 
        {

        }

    }

    /// <summary>
    /// Represents a weapon. Spawns GameObjects with AttackComponents for the actual attack or bullet firing.
    /// </summary>
    
    class WeaponComponent : Component
    {

        public float AttackDelay { get; set; }
        
        protected double LastAttack;
        protected double Time;
        protected Vector2f Position;
        protected Vector2f AttackDirection;
        protected GameObject Owner;

        public WeaponComponent(GameObject owner, GameObject parent) : base(parent)
        {
            AttackDirection = new Vector2f(1, 0);
            Owner = owner;
        }

        public override void Update(Engine.GameTime gameTime)
        {
            Time = gameTime.TotalGameTime.TotalSeconds;
            Position = Owner.Position; // + offset
        }

        public virtual bool TryAttack() { return false; }


        public override void HandleMessage(Message message)
        {
            base.HandleMessage(message);

            if (message is AttackMessage)
            {
                AttackMessage attackMsg = message as AttackMessage;
                TryAttack();
            }
            if(message is WalkLeftMessage)
            {
                AttackDirection = new Vector2f(-1, 0);
            }
            if(message is WalkRightMessage)
            {
                AttackDirection = new Vector2f(1, 0);
            }
        }
    }

    class MeleeWeaponComponent : WeaponComponent
    {
        public MeleeWeaponComponent(GameObject owner, GameObject parent) : base(owner, parent) { }

        public override bool TryAttack()
        {
            if (Time > LastAttack + AttackDelay)
            {
                LastAttack = Time;
                GameObjectFactory.CreateSword(Owner, Position, 95, 35, Math.Sign(AttackDirection.X));
                return true;
            }

            return false;
        }
    }

    class GunWeaponComponent : WeaponComponent 
    {
        public GunWeaponComponent(GameObject owner, GameObject parent) : base(owner, parent) {  }

        public override bool TryAttack()
        {
            if (Time > LastAttack + AttackDelay)
            {
                LastAttack = Time;
                GameObjectFactory.CreateBullet(Owner, Position + AttackDirection * 40.0f, AttackDirection);
                return true;
            }

            return false;
        }
    }
}
