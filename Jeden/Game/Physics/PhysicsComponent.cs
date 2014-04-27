using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace Jeden.Game.Physics
{
    class PhysicsComponent : Component
    {
        //TODO: integrate Farseer physics
        public Body BoundingBox { get; set; }

        public PhysicsComponent(GameObject parent, Body box) : base(parent)
        {
            BoundingBox = box;
        }

        /// <summary>
        /// Update owning GameObject's position with this components position
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(Engine.GameTime gameTime)
        {
            base.Update(gameTime);
            Parent.Position.X = BoundingBox.Position.X;
            Parent.Position.Y = BoundingBox.Position.Y;
        }
    }
}
