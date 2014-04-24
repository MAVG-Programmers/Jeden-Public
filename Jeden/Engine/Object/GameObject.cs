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
        public Vector2i Position { get; set; }

        /// <summary>
        /// A new instance of GameObject.
        /// </summary>
        public GameObject()
        {
            Components = new Dictionary<Type, Component>();
            AddComponent(new HealthComponent(this, 1)); //Default hp 1
        }

        /// <summary>
        /// Updates the GameObject and all of its components.
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Checks whether the GameObject owns a Component of the type T.
        /// </summary>
        /// <typeparam name="T">The checked component type.</typeparam>
        /// <returns>true if GameObject owns a Component of type T; false otherwise.</returns>
        public bool ContainsComponent<T>() where T : Component
        {
            return Components.ContainsKey(typeof(Task));
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
    }
}
