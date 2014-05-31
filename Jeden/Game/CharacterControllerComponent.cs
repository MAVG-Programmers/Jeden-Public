using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public float InAirMovementImpulse;
        public float JumpImpulse;
        public float WalkingLinearDamping;
        public float InAirLinearDamping;
        Camera Camera;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">GameObect that owns the component</param>
        public CharacterControllerComponent(AnimationSetRenderComponent animSetComponent, 
            PhysicsComponent physicsComponent,
            Camera camera, // for camera shake, needs to be moved if there are other "Characters" than the player.
            GameObject parent) : base(parent)
        {
            AnimationSetRenderComponent = animSetComponent;
            PhysicsComponent = physicsComponent;

            Camera = camera;

            FeetColliders = new List<GameObject>();

            Debug.Assert(AnimationSetRenderComponent != null);
            Debug.Assert(PhysicsComponent != null);
        }

        public override void Update(Engine.GameTime gameTime)
        {
            InAir = FeetColliders.Count == 0;

            if (AnimationSetRenderComponent.CurrentKey == "Attacking" && !AnimationSetRenderComponent.IsFinished())
            {

            }
            else
            {

                if (InAir && AnimationSetRenderComponent.CurrentKey != "Jumping")
                {
                    AnimationSetRenderComponent.SetAnimation("Falling");
                }
                if (AnimationSetRenderComponent.CurrentKey == "Jumping" && AnimationSetRenderComponent.IsFinished())
                {
                    AnimationSetRenderComponent.SetAnimation("Falling");
                }

                if (!IsWalking && !InAir)
                    AnimationSetRenderComponent.SetAnimation("Idle");
            }

            IsWalking = false;
        }

        public override void HandleMessage(Message message)
        {

            if (message is WalkLeftMessage)
            {
                if (!InAir)
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(-WalkImpulse, 0));
                else
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(-InAirMovementImpulse, 0));

                AnimationSetRenderComponent.FlipX = true;
                if (AnimationSetRenderComponent.CurrentKey != "Jumping" && 
                    AnimationSetRenderComponent.CurrentKey != "Falling" && 
                    AnimationSetRenderComponent.CurrentKey != "Attacking")
                {
                    IsWalking = true;
                    AnimationSetRenderComponent.SetAnimation("Walking"); 
                }
            }
            else if (message is WalkRightMessage)
            {
                if (!InAir)
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(WalkImpulse, 0));
                else
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(InAirMovementImpulse, 0));

                AnimationSetRenderComponent.FlipX = false;
                if (AnimationSetRenderComponent.CurrentKey != "Jumping" && 
                    AnimationSetRenderComponent.CurrentKey != "Falling" && 
                    AnimationSetRenderComponent.CurrentKey != "Attacking")
                {
                    IsWalking = true;
                    AnimationSetRenderComponent.SetAnimation("Walking");
                }
            }
            else if (message is JumpMessage)
            {
                if (FeetColliders.Count > 0)
                {
                    //FeetColliders.Clear();
                    PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(0.0f, -JumpImpulse));
                    AnimationSetRenderComponent.SetAnimation("Jumping");
                }
            }

            if (message is DamageMessage)
            {
                Camera.Shake();
            }

            if (message is AttackMessage)
            {
                //TODO: this is ugly.
                foreach (Component comp in Parent.Components)
                {
                    if (comp is WeaponHoldingComponent)
                    {

                        WeaponHoldingComponent weaponHoldingComp = comp as WeaponHoldingComponent;


                        AttackMessage attackMessage = message as AttackMessage;
                        {
                            if (attackMessage.Melee)
                            {
                                if (weaponHoldingComp.MeleeWeaponComponent.TryAttack())
                                {
                                    AnimationSetRenderComponent.SetAnimation("Attacking");
                                }
                            }
                            else
                            {
                                if (weaponHoldingComp.GunWeaponComponent.TryAttack())
                                {
                                    // do nothing for gun attack.
                                    //AnimationSetRenderComponent.SetAnimation("Attacking");
                                }
                            }
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

                if(collisionMsg.Contact.Manifold.LocalNormal.Y > 0) // only friction the floor
                {
                    FeetColliders.Add(collisionMsg.GameObject);
                    if (AnimationSetRenderComponent.CurrentKey == "Jumping" ||
                        AnimationSetRenderComponent.CurrentKey == "Falling" &&
                        AnimationSetRenderComponent.CurrentKey != "Attacking")
                    {
                        AnimationSetRenderComponent.SetAnimation("Idle");
                    }
                }
                else // don't friction walls or ceilings
                {
                    collisionMsg.Contact.Friction = 0.0f;
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


            InAir = FeetColliders.Count == 0;

            if (InAir)
                PhysicsComponent.Body.LinearDamping = InAirLinearDamping;
            else
                PhysicsComponent.Body.LinearDamping = WalkingLinearDamping;
        }


        AnimationSetRenderComponent AnimationSetRenderComponent; 
        PhysicsComponent PhysicsComponent;
        bool InAir;
        bool IsWalking;
        

        List<GameObject> FeetColliders; 
    }
}
