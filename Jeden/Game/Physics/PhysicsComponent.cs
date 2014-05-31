using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine.Object;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace Jeden.Game.Physics
{
    class PhysicsComponent : Component
    {
        //TODO: integrate Farseer physics
        public Body Body { get; set; }

        public PhysicsComponent(Body body, PhysicsManager physicsMgr, GameObject parent) : base(parent)
        {
            Manager = physicsMgr;
            Body = body;
        }

        /// <summary>
        /// Update owning GameObject's position with this components position
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(Engine.GameTime gameTime)
        {
            base.Update(gameTime);
            Parent.Position.X = Body.Position.X;
            Parent.Position.Y = Body.Position.Y;
        }
    }
}
