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
        public bool Melee;
        public AttackMessage(Component sender, bool melee) : base(sender) 
        {
            Melee = melee;
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
        protected int AttackDirection;
        protected GameObject Owner;
        protected Vector2f Offset;
        protected Vector2f SpawnOffset;

        public WeaponComponent(GameObject owner, Vector2f offset, GameObject parent) : base(parent)
        {
            AttackDirection = 1;
            Owner = owner;
            Offset = offset;
            SpawnOffset = offset;
        }

        public override void Update(Engine.GameTime gameTime)
        {
            Time = gameTime.TotalGameTime.TotalSeconds;
            Parent.Position = Owner.Position + new Vector2f(Offset.X * AttackDirection, Offset.Y);
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
                AttackDirection = -1;
            }
            if(message is WalkRightMessage)
            {
                AttackDirection = 1;
            }
        }
    }

    class MeleeWeaponComponent : WeaponComponent
    {
        public MeleeWeaponComponent(GameObject owner, Vector2f offset, GameObject parent) : base(owner, offset, parent) { }

        public override bool TryAttack()
        {
            if (Time > LastAttack + AttackDelay)
            {
                LastAttack = Time;
                Vector2f actualOffset = new Vector2f(Offset.X * AttackDirection, Offset.Y);
                GameObjectFactory.CreateJab(Owner, 
                    this, 
                    Owner.Position + actualOffset, 
                    AttackDirection);
                return true;
            }

            return false;
        }
    }

    class GunWeaponComponent : WeaponComponent 
    {
        public GunWeaponComponent(GameObject owner, Vector2f offset, GameObject parent) : base(owner, offset, parent) {  }

        public override bool TryAttack()
        {
            if (Time > LastAttack + AttackDelay)
            {
                LastAttack = Time;
                Vector2f actualOffset = new Vector2f(Offset.X * AttackDirection, Offset.Y);
                GameObjectFactory.CreateBullet(
                    Owner, 
                    Owner.Position + actualOffset, 
                    AttackDirection);
                return true;
            }

            return false;
        }
    }
}
