using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Engine.Render;
using Jeden.Game.Physics;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Jeden.Game
{

    class WalkLeftMessage : Message
    {
        public WalkLeftMessage(Object sender) : base(sender) { }
    }

    class WalkRightMessage : Message
    {
        public WalkRightMessage(Object sender) : base(sender) { }
    }

    class JumpMessage : Message
    {
        public JumpMessage(Object sender) : base(sender) { }
    }

    // Should this be split up between all the components it controls?


    /// <summary>
    /// Controls "Characters", things that Walk, Jump, Attack, Die and other "Character" actions.
    /// </summary>
    class CharacterControllerComponent : Component
    {


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">GameObect that owns the component</param>
        public CharacterControllerComponent(GameObject parent) : base(parent)
        {
            AnimationSetRenderComponent = (AnimationSetRenderComponent) Parent.GetComponent<RenderComponent>();
            PhysicsComponent = Parent.GetComponent<PhysicsComponent>();

            FeetColliders = new List<GameObject>();

            Debug.Assert(AnimationSetRenderComponent != null);
            Debug.Assert(PhysicsComponent != null);
        }

        public override void Update(Engine.GameTime gameTime)
        {
            if(AnimationSetRenderComponent.IsFinished())
            {
                AnimationSetRenderComponent.SetAnimation("Walking");
            }
        }

        public override void HandleMessage(Message message)
        {
            //TODO: get magic numbers out of here.
            if(message is WalkLeftMessage)
            {
                PhysicsComponent.BoundingBox.ApplyLinearImpulse(new Vector2(-9000.0f, 0));
                AnimationSetRenderComponent.FlipX = true;
            }


            if(message is WalkRightMessage)
            {
                PhysicsComponent.BoundingBox.ApplyLinearImpulse(new Vector2(9000.0f, 0));
                AnimationSetRenderComponent.FlipX = false;
            }
            if(message is JumpMessage)
            {
                if (FeetColliders.Count > 0)
                {
                    PhysicsComponent.BoundingBox.ApplyLinearImpulse(new Vector2(0.0f, -1600000));
                }
            }

            if (message is AttackMessage)
            {

                WeaponHoldingComponent weaponHoldingComp = Parent.GetComponent<WeaponHoldingComponent>();
                WeaponComponent weapon = weaponHoldingComp.WeaponComponent;

                if (weapon.TryAttack())
                {
                    AnimationSetRenderComponent.SetAnimation("Attacking");
                }
            }

            if(message is DeathMessage)
            {
                GameObjectFactory.CreateDeadGuy(Parent.Position);
                Parent.Valid = false;
            }
            if(message is CollisionMessage)
            {
                CollisionMessage collisionMsg = message as CollisionMessage;
                if(collisionMsg.Contact.Manifold.LocalNormal.Y > 0)
                {
                    FeetColliders.Add(collisionMsg.GameObject);
                }
            }
            if (message is SeperationMessage)
            {
                SeperationMessage seperationMsg = message as SeperationMessage;
                if (FeetColliders.Contains(seperationMsg.GameObject))
                {
                    FeetColliders.Remove(seperationMsg.GameObject);
                }
            }
        }


        AnimationSetRenderComponent AnimationSetRenderComponent; 
        PhysicsComponent PhysicsComponent;
        

        List<GameObject> FeetColliders; 
    }
}
