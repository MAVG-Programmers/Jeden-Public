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
using FarseerPhysics.Dynamics.Contacts;


namespace Jeden.Game.Physics
{

    

    class CollisionMessage : Message
    {
        public CollisionMessage(GameObject gameObject, Contact contact, Object sender) : base(sender)
        {
            GameObject = gameObject;
            Contact = contact;
        }

        public GameObject GameObject;
        public Contact Contact;
    }

    class SeperationMessage : Message
    {
        public SeperationMessage(GameObject gameObject, Object sender)
            : base(sender)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject;
    }

    class PhysicsManager
    {
        public const Category PlayerCategory = Category.Cat1;
        public const Category EnemyCategory = Category.Cat2;
        public const Category MapCategory = Category.Cat3;

        //TODO: integrate Farseer physics
        List<PhysicsComponent> Components;
        private World _world;

        public PhysicsManager()
        {
            Components = new List<PhysicsComponent>();
            //Create world with regular gravity
            _world = new World(new Vector2(0f, 149.82f));
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

        public PhysicsComponent MakeNewComponent(GameObject owner, float width, float height, Category categories, Category collidesWith, bool dynamic)
        {

           
            //Fixed density of 1.0
            var body = BodyFactory.CreateRectangle(_world, width, height, 1.0f);
            body.Position = new Vector2(owner.Position.X, owner.Position.Y);
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
            body.UserData = owner;
            body.OnCollision += OnFixtureCollision;
            body.OnSeparation += OnFixtureSeperation;
            body.CollidesWith = collidesWith;
            body.CollisionCategories = categories;

            body.LinearDamping = 1;
            
            Components.Add(comp);
            return comp;
        }

        public void RemoveComponent(PhysicsComponent comp)
        {
            //Remove from Farseer world

            if (_world.BodyList.Contains(comp.BoundingBox))
                _world.RemoveBody(comp.BoundingBox);

            //Remove from Components list
            if (Components.Contains(comp))
            {
                Components.Remove(comp);
            }
        }

        bool OnFixtureCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            GameObject gameObjectA = (GameObject)fixtureA.Body.UserData;
            GameObject gameObjectB = (GameObject)fixtureB.Body.UserData;

            gameObjectA.HandleMessage(new CollisionMessage(gameObjectB, contact, this));

            return true; // TODO: how to handle this???
        }

        void OnFixtureSeperation(Fixture fixtureA, Fixture fixtureB)
        {
            GameObject gameObjectA = (GameObject)fixtureA.Body.UserData;
            GameObject gameObjectB = (GameObject)fixtureB.Body.UserData;

            gameObjectA.HandleMessage(new SeperationMessage(gameObjectB, this));
        }
    }
}
