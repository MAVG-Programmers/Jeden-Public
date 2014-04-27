using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine;
using Jeden.Engine.Object;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;


namespace Jeden.Game.Physics
{
    class PhysicsManager
    {
        //TODO: integrate Farseer physics
        List<PhysicsComponent> Components;
        private World _world;

        public PhysicsManager()
        {
            Components = new List<PhysicsComponent>();
            //Create world with regular gravity
            _world = new World(new Vector2(0f, 9.82f));
        }

        //Update tick all components owned by this manager
        public void Update(GameTime gameTime)
        {
            _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var comp in Components)
            {
               comp.Update(gameTime);
            }
        }

        public PhysicsComponent MakeNewComponent(GameObject owner, float x, float y, float width, float height, bool dynamic)
        {
            //Fixed density of 1.0
            var body = BodyFactory.CreateRectangle(_world, width, height, 1.0f);
            body.Position = new Vector2(x, y);
            if (dynamic)
            {
                body.BodyType = BodyType.Dynamic;
            }
            else
            {
                body.BodyType = BodyType.Static;
            }
            //AABB, no rotation
            body.FixedRotation = true;
            PhysicsComponent comp = new PhysicsComponent(owner, body);
            Components.Add(comp);
            return comp;
        }

        public void RemoveComponent(PhysicsComponent comp)
        {
            //Remove from Farseer world
            _world.RemoveBody(comp.BoundingBox);

            //Remove from Components list
            if (Components.Contains(comp))
            {
                Components.Remove(comp);
            }
        }
    }
}
