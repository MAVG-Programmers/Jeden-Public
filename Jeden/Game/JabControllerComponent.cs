using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine.Object;
using SFML.Window;
using System.Diagnostics;
using Jeden.Game.Physics;

namespace Jeden.Game
{
    class JabControllerComponent : Component 
    {
        // moves the object from ForwardPosition to BackwardPosition. 
        //The object will reach ForwardPosition at FullForwardTime and return back to BackwardPosition at FullBackwardTime 

        public Vector2f ForwardPosition; // in local space
        public Vector2f BackwardPosition; // in local space
        double FullForwardTime;
        double FullBackwardTime;
        double Time;
        PhysicsComponent PhysicsComponent;
        MeleeWeaponComponent MeleeWeapon;

        public JabControllerComponent(PhysicsComponent physicsComp, MeleeWeaponComponent meleeWeapon,
            Vector2f forwardPosition, Vector2f backwardPosition,
            double fullForwardTime, double fullBackwardTime, 
            GameObject parent) : base(parent)
        {
            ForwardPosition = forwardPosition;
            BackwardPosition = backwardPosition;
            FullForwardTime = fullForwardTime;
            FullBackwardTime = fullBackwardTime;
            Time = 0;
            PhysicsComponent = physicsComp;
            MeleeWeapon = meleeWeapon;

          //  Debug.Assert(FullBackwardTime > fullForwardTime);
            
        }

        public override void Update(Engine.GameTime gameTime)
        {
            base.Update(gameTime);

            Time += gameTime.ElapsedGameTime.TotalSeconds;

            if(Time < FullForwardTime) // move forward
            {
                double t = Time / FullForwardTime;
                Vector2f p = MeleeWeapon.Parent.Position + BackwardPosition + (float)t * (ForwardPosition - BackwardPosition);
                PhysicsComponent.Body.Position = new Microsoft.Xna.Framework.Vector2(p.X, p.Y);
            }
            else if(Time < FullBackwardTime) // movebackward
            {
                double t = (Time - FullForwardTime) / FullBackwardTime;
                Vector2f p = MeleeWeapon.Parent.Position + ForwardPosition + (float)t * (BackwardPosition - ForwardPosition);
                PhysicsComponent.Body.Position = new Microsoft.Xna.Framework.Vector2(p.X, p.Y);
            }
            else // finished, destroy self
            {
                Parent.HandleMessage(new InvalidateMessage(this));
            }
        }
    
    }
}
