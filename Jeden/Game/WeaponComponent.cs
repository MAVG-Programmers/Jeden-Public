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
        JedenGameState GameState;

        static Texture Texture = new Texture("assets/player.png");

        public float AttackRate { get; set; }
        double LastAttack;
        double Time;
        Vector2f Position;
        Vector2f AttackDirection;
        GameObject Owner;

        public WeaponComponent(JedenGameState gameState, GameObject owner, GameObject parent) : base(parent)
        {
            AttackDirection = new Vector2f(1, 0);
            Owner = owner;
            GameState = gameState;
        }

        public override void Update(Engine.GameTime gameTime)
        {
            Time = gameTime.TotalGameTime.TotalSeconds;
            Position = Owner.Position; // + offset
        }

        bool TryAttack()
        {
            if (Time > LastAttack + AttackRate)
            {
                LastAttack = Time;
                GameObjectFactory.CreateBullet(Owner, Position + AttackDirection * 40.0f, AttackDirection);
                return true;
            }

            return false;
        }

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
}
