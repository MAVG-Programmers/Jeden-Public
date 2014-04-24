using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Object
{
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
        public HealthComponent(GameObject parent, float maxHealth, float regenerationPerSecond) : base(parent)
        {
            MaxHealth = maxHealth;
            RegenerationPerSecond = regenerationPerSecond;
        }

        /// <summary>
        /// Reduces the Health of the HealthComponent.
        /// </summary>
        /// <param name="amount">The amount of damage.</param>
        public void Damage(float amount)
        {
            //TODO: Calculate resistances to damage taken
            CurrentHealth -= amount;
        }

        /// <summary>
        /// Updates the HealthComponent
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public override void Update(GameTime gameTime)
        {
            if (CurrentHealth <= 0)
            {
                //TODO: Throw event for death
            }
            else
            {
                //Check if cur health is < than max, if true regen health
                if (CurrentHealth <= MaxHealth) 
                {
                    CurrentHealth += ( RegenerationPerSecond / 1000 ) * gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            //Truncate cur health to max health
            if (CurrentHealth > MaxHealth) 
            {
                CurrentHealth = MaxHealth;
            }
        }
    }
}
