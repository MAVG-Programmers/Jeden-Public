using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Game.Physics;
using Jeden.Engine.Render;
using SFML.Window;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Jeden.Game
{

    class FlyMessage : Message
    {
        public FlyMessage(Vector2f direction, Object sender) : base(sender)
        {
            Direction = direction;
        }

        public Vector2f Direction;
    }

    class FlyingBugControllerComponent : Component 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">GameObect that owns the component</param>
        public FlyingBugControllerComponent(AnimationSetRenderComponent animSetComponent, PhysicsComponent physicsComponent, GameObject parent)
            : base(parent)
        {

            AnimationSetRenderComponent = animSetComponent;
            PhysicsComponent = physicsComponent;

            Debug.Assert(AnimationSetRenderComponent != null);
            Debug.Assert(PhysicsComponent != null);
            AnimationSetRenderComponent.SetAnimation("Walking");
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

            if(message is FlyMessage)
            {
                FlyMessage flyMessage = message as FlyMessage;
                PhysicsComponent.Body.ApplyLinearImpulse(new Vector2(flyMessage.Direction.X, flyMessage.Direction.Y) * 200);
            }

            if(message is InvalidatedMessage)
            {
                GameObjectFactory.CreateDeadBug(Parent.Position);
            }
        }


        AnimationSetRenderComponent AnimationSetRenderComponent; 
        PhysicsComponent PhysicsComponent;
    }
}
