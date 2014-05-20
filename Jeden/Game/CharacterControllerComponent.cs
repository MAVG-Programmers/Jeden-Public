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
        public float WalkImpulse;
        public float JumpImpulse;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">GameObect that owns the component</param>
        public CharacterControllerComponent(AnimationSetRenderComponent animSetComponent, PhysicsComponent physicsComponent, GameObject parent) : base(parent)
        {
            AnimationSetRenderComponent = animSetComponent;
            PhysicsComponent = physicsComponent;

            FeetColliders = new List<GameObject>();

            Debug.Assert(AnimationSetRenderComponent != null);
            Debug.Assert(PhysicsComponent != null);
        }

        public override void Update(Engine.GameTime gameTime)
        {
            const float MaxVelocity = 20;
            if(PhysicsComponent.Body.LinearVelocity.LengthSquared() > MaxVelocity * MaxVelocity)
            {
                PhysicsComponent.Body.LinearVelocity = PhysicsComponent.Body.LinearVelocity / PhysicsComponent.Body.LinearVelocity.Length();
                PhysicsComponent.Body.LinearVelocity = PhysicsComponent.Body.LinearVelocity * MaxVelocity;
            }

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
                PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(-WalkImpulse, 0));
                AnimationSetRenderComponent.FlipX = true;
            }


            if(message is WalkRightMessage)
            {
                PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(WalkImpulse, 0));
                AnimationSetRenderComponent.FlipX = false;
            }
            if(message is JumpMessage)
            {
                if (FeetColliders.Count > 0)
                {
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(0.0f, -JumpImpulse));
                }
            }

            if (message is AttackMessage)
            {

                foreach (Component comp in Parent.Components)
                {
                    if (comp is WeaponHoldingComponent)
                    {
                        WeaponHoldingComponent weaponHoldingComp = comp as WeaponHoldingComponent;
                        WeaponComponent weapon = weaponHoldingComp.WeaponComponent;

                        if (weapon.TryAttack())
                        {
                            AnimationSetRenderComponent.SetAnimation("Attacking");
                        }
                    }
                }
            }

            if(message is InvalidateMessage)
            {
                GameObjectFactory.CreateDeadGuy(Parent.Position);
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
