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

    class DeathMessage : Message
    {
        public DeathMessage(Component sender) : base(sender) { }
    }

    /// <summary>
    /// Represents a GameObjects health, armor and resistances.
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

        /// <summary>
        /// The amount of health that is regenerated each second.
        /// </summary>
        public float RegenerationPerSecond { get; set; }

        /// <summary>
        /// A new instance of HealthComponent.
        /// </summary>
        /// <param name="parent">The GameObject that owns this HealthComponent.</param>
        /// <param name="hp">The parent's maximum health.</param>
        public HealthComponent(GameObject parent, float maxHealth, float regenerationPerSecond)
            : base(parent)
        {
            MaxHealth = maxHealth;
            RegenerationPerSecond = regenerationPerSecond;
            CurrentHealth = maxHealth;
        }

        /// <summary>
        /// Reduces the Health of the HealthComponent.
        /// </summary>
        /// <param name="amount">The amount of damage.</param>
        public void Damage(float amount)
        {
            //TODO: Calculate resistances to damage taken
            CurrentHealth -= amount;

            Parent.HandleMessage(new DamageMessage(this, amount));
        }

        /// <summary>
        /// Updates the HealthComponent
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public override void Update(GameTime gameTime)
        {
            if (CurrentHealth <= 0)
            {
                Parent.Valid = false; // HACK.
                Parent.HandleMessage(new DeathMessage(this));
            }
            else
            {
                //Check if cur health is < than max, if true regen health
                if (CurrentHealth <= MaxHealth)
                {
                    CurrentHealth += (RegenerationPerSecond / 1000) * gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            //Truncate cur health to max health
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
                if(collisionMsg.GameObject.ContainsComponent<AttackComponent>())
                {
                    AttackComponent attackComp = collisionMsg.GameObject.GetComponent<AttackComponent>();
                    Damage(attackComp.Damage);
                }
            }
        }
    }


}
