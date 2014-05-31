using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML;
using SFML.Window;

namespace Jeden.Engine.Object
{

    class InvalidateMessage : Message
    {
        public InvalidateMessage(object sender) : base(sender)
        {

        }
    }

    /// <summary>
    /// Represents a object in the game.
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// Represents the GameObject's behavior
        /// </summary>
        public List<Component> Components;

        /// <summary>
        /// The position of the GameObject in the game world.
        /// </summary>
        public Vector2f Position;

        public bool Valid {get; private set;} // Destroy a game object by setting valid to false. The object will be allowed to cary out the frame. 
                    // The last thing a frame does is remove all invalid objects. It is the responsibility of objects holding refrences to
                    // GameObjects to check there Valid before attempting to manipulate them.


        /// <summary>
        /// A new instance of GameObject.
        /// </summary>
        public GameObject()
        {
            Components = new List<Component>();
            Position = new Vector2f(0.0f, 0.0f);
            Valid = true;
        }

        /// <summary>
        /// Updates the GameObject and all of its components.
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            foreach(Component comp in Components)
            {
                if(comp.Manager == null) // Managers do their own updates
                    comp.Update(gameTime);
            }
        }

        /// <summary>
        /// Adds a Component to the GameObject.
        /// </summary>
        /// <typeparam name="T">The Component's type.</typeparam>
        /// <param name="component">The Component that is added.</param>
        public void AddComponent(Component comp)
        {
            Components.Add(comp);
           
        }

        /// <summary>
        /// Removes a Component from the GameObject
        /// </summary>
        /// <typeparam name="T">The Component type that will be removed.</typeparam>
        public void RemoveComponent(Component comp) 
        {
            Components.Remove(comp);
        }

        /// <summary>
        /// Call this to send the component a message, or override it to handle a message.
        /// </summary>
        /// <param name="message">The message</param>
        public void HandleMessage(Message message)
        {
            if (message is InvalidateMessage)
                Valid = false;

            foreach(Component comp in Components)
            {
                if (comp != message.Sender)
                    comp.HandleMessage(message);
            }
        }
    }
}
