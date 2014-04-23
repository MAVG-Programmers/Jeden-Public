using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;

namespace Jeden.Engine.Object
{
    [Serializable]
    public class GameObject
    {
        public Dictionary<Type, Component> Components { get; set; } //TODO: allow multiple components of same type?
        public Vector2i Position { get; set; }

        public GameObject()
        {
            Components = new Dictionary<Type, Component>();
            AddComponent(new HealthComponent(this, 1)); //Default hp 1
        }

        public virtual void Update(int dTime)
        {

        }

        public bool ContainsComponent<T>() where T : Component
        {
            return Components.ContainsKey(typeof(Task));
        }
        public T GetComponent<T>() where T : Component 
        {
            if (Components.ContainsKey(typeof(T))) 
            {
                return (T) Components[typeof(T)];
            }
            return null;
        }
        public void AddComponent<T>(T component) where T : Component 
        {
            if (!Components.ContainsKey(typeof(T))) 
            {
                Components.Add(typeof(T), component);
            }
        }
        public void RemoveComponent<T>() where T : Component 
        {
            Components.Remove(typeof(T));
        }
        //represents everything in the game (enemies, Playercharacter, Crates/Boxes, ...)
    }
}
