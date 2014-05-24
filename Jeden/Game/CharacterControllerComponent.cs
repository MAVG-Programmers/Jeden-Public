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
        }

        public override void HandleMessage(Message message)
        {
            //TODO: get magic numbers out of here.
            if (message is WalkLeftMessage)
            {
                PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(-WalkImpulse, 0));
                AnimationSetRenderComponent.FlipX = true;
                AnimationSetRenderComponent.SetAnimation("Walking"); // this will just set to falling later if the character is falling
            }
            else if (message is WalkRightMessage)
            {
                PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(WalkImpulse, 0));
                AnimationSetRenderComponent.FlipX = false;
                AnimationSetRenderComponent.SetAnimation("Walking");// this will just set to falling later if the character is falling
            }
            else if (message is JumpMessage)
            {
                if (FeetColliders.Count > 0)
                {
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(0.0f, -JumpImpulse));
                    AnimationSetRenderComponent.SetAnimation("Jumping");
                }
            }
            else
                AnimationSetRenderComponent.SetAnimation("Idle");// this will just set to falling later if the character is falling

            if (FeetColliders.Count == 0) // in the air
            {
                if (AnimationSetRenderComponent.CurrentKey == "Jumping")
                {
                    if (AnimationSetRenderComponent.IsFinished()) // finished jump animation so switch to falling
                        AnimationSetRenderComponent.SetAnimation("Falling");
                }
                else
                {
                    AnimationSetRenderComponent.SetAnimation("Falling");
                }
            }

            if (message is DamageMessage)
            {

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
                GameObjectFactory.CreateDeadPlayer(Parent.Position);
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
