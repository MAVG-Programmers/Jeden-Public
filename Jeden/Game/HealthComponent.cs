using System;
using System.Collections.Generic;
using Jeden.Engine;
using Jeden.Engine.Object;
using Jeden.Game.Physics;

namespace Jeden.Game
{

    class DamageMessage : Message 
    {
        float Damage { get; set; }

        public DamageMessage(Component sender, float damage) : base(sender)
        {
            Damage = damage;
        }
    }


    /// <summary>
    /// Represents a GameObjects health
    /// </summary>
    class HealthComponent : Component
    {
        /// <summary>
        /// The parent's maximum health.
        /// </summary>
        public float MaxHealth { get; set; }

        /// <summary>
        /// The parent's current health.
        /// </summary>
        public float CurrentHealth { get; set; }


        public HealthComponent(GameObject parent, float maxHealth)
            : base(parent)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentHealth <= 0)
            {
                Parent.HandleMessage(new InvalidateMessage(this));
            }

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public override void HandleMessage(Message message)
        {
            base.HandleMessage(message);

            if(message is CollisionMessage)
            {

                CollisionMessage collisionMsg = message as CollisionMessage;
                foreach(Component comp in collisionMsg.GameObject.Components)
                {
                    if (comp is AttackComponent)
                    {
                        AttackComponent attackComp = comp as AttackComponent;

                        CurrentHealth -= attackComp.Damage;

                        Parent.HandleMessage(new DamageMessage(this, attackComp.Damage));

                        GameObjectFactory.CreateShieldDamgageEffect(Parent.Position);
                    }
                }
            }
        }
    }


}
