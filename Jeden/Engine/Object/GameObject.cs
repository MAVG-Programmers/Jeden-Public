using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;

namespace Jeden.Engine.Object
{
    /// <summary>
    /// Represents a object in the game.
    /// </summary>
    [Serializable]
    public class GameObject
    {
        /// <summary>
        /// Represents the GameObject's behavior
        /// </summary>
        public Dictionary<Type, Component> Components { get; set; } //TODO: allow multiple components of same type?

        /// <summary>
        /// The position of the GameObject in the game world.
        /// </summary>
        public Vector2f Position;
        public GameState OwningState { get; set; }
        public bool Valid {get; set;} // Destroy a game object by setting valid to false. The object will be allowed to cary out the frame. 
                    // The last thing a frame does is remove all invalid objects. It is the responsibility of objects holding refrences to
                    // GameObjects to check there Valid before attempting to manipulate them.

        /// <summary>
        /// A new instance of GameObject.
        /// </summary>
        public GameObject(GameState owner)
        {
            Components = new Dictionary<Type, Component>();
            OwningState = owner;
            Position = new Vector2f(0.0f, 0.0f);
            Valid = true;
        }

        /// <summary>
        /// Updates the GameObject and all of its components.
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            //THIS IS NOT RIGHT!!
            // DOUBLE UPDATES COMPONENTS WITH MANAGER!
            foreach(Component comp in Components.Values)
            {
                comp.Update(gameTime);
            }
        }

        /// <summary>
        /// Checks whether the GameObject owns a Component of the type T.
        /// </summary>
        /// <typeparam name="T">The checked component type.</typeparam>
        /// <returns>true if GameObject owns a Component of type T; false otherwise.</returns>
        public bool ContainsComponent<T>() where T : Component
        {
            return Components.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Checks whether the GameObject owns a Component of type T and returns it.
        /// </summary>
        /// <typeparam name="T">The checked component type.</typeparam>
        /// <returns>If GameObject owns a Component of type T it is returned; null otherwise.</returns>
        public T GetComponent<T>() where T : Component 
        {
            if (Components.ContainsKey(typeof(T))) 
            {
                return (T) Components[typeof(T)];
            }
            return null;
        }

        /// <summary>
        /// Adds a Component of type T to the GameObject.
        /// </summary>
        /// <typeparam name="T">The Component's type.</typeparam>
        /// <param name="component">The Component that is added.</param>
        public void AddComponent<T>(T component) where T : Component 
        {
            if (!Components.ContainsKey(typeof(T))) 
            {
                Components.Add(typeof(T), component);
            }
        }

        /// <summary>
        /// Removes a Component of the type T from the GameObject
        /// </summary>
        /// <typeparam name="T">The Component type that will be removed.</typeparam>
        public void RemoveComponent<T>() where T : Component 
        {
            Components.Remove(typeof(T));
        }

        /// <summary>
        /// Call this to send the component a message, or override it to handle a message.
        /// </summary>
        /// <param name="message">The message</param>
        public void HandleMessage(Message message)
        {
            foreach(Component comp in Components.Values)
            {
                if (comp != message.Sender)
                    comp.HandleMessage(message);
            }
        }
    }
}
