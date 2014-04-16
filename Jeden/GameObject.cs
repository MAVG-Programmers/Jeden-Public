using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;

namespace Jeden
{
    [Serializable]
    public class GameObject
    {
        public Dictionary<Type, Component> Components { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public GameObject() 
        {
            Components = new Dictionary<Type, Component>();
            Position = new Vector2();
        }

        public virtual void LoadContent(JedenGame game)
        {
            foreach (KeyValuePair<Type, Component> component in Components) 
            {
                component.Value.LoadContent(game);
            }
        }
        public virtual void Update(JedenGame game)
        {
            foreach (KeyValuePair<Type, Component> component in Components)
            {
                component.Value.Update(game);
            }
        }
        public virtual void Draw(JedenGame game)
        {
            foreach (KeyValuePair<Type, Component> component in Components)
            {
                component.Value.Draw(game);
            }
        }
        public virtual void UnloadContent(JedenGame game)
        {
            foreach (KeyValuePair<Type, Component> component in Components)
            {
                component.Value.UnloadContent(game);
            }
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
