using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine;
using Jeden.Engine.Object;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;


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

    class PhysicsManager : Manager 
    {
        public const Category PlayerCategory = Category.Cat1;
        public const Category EnemyCategory = Category.Cat2;
        public const Category MapCategory = Category.Cat3;

        List<PhysicsComponent> Components;
        private World _world;

        public PhysicsManager()
        {
            Components = new List<PhysicsComponent>();

            ConfigFileParser physicsConfigFileParser = new ConfigFileParser("cfg/physics_world.txt");

        
            _world = new World(new Vector2(0f, physicsConfigFileParser.GetAsFloat("Gravity")));

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

        public PhysicsComponent MakeNewComponent(GameObject owner, float width, float height, Category categories, Category collidesWith, BodyType bodyType)
        {

           
            //Fixed density of 1.0
            var body = BodyFactory.CreateRectangle(_world, width, height, 1.0f);
            body.Position = new Vector2(owner.Position.X, owner.Position.Y);

            body.BodyType = bodyType;
           

            //AABB, no rotation
            body.FixedRotation = true;
            PhysicsComponent comp = new PhysicsComponent(body, this, owner);
            body.UserData = owner;
            body.OnCollision += OnFixtureCollision;
            body.OnSeparation += OnFixtureSeperation;
            body.CollidesWith = collidesWith;
            body.CollisionCategories = categories;

            body.LinearDamping = 1;
            
            Components.Add(comp);
            return comp;
        }

        public override void RemoveComponent(Component comp)
        {
            Debug.Assert(comp is PhysicsComponent);

            PhysicsComponent physicsComp = comp as PhysicsComponent;
            //Remove from Farseer world

            if (_world.BodyList.Contains(physicsComp.Body))
                _world.RemoveBody(physicsComp.Body);

            //Remove from Components list
            if (Components.Contains(physicsComp))
            {
                Components.Remove(physicsComp);
            }
        }

        // Converts Farseer messages to game messages.
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
