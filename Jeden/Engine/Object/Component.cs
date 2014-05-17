using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Object
{
    /// <summary>
    /// Represents a part of a GameObject.
    /// </summary>
    [Serializable]
    public class Component
    {

        public Manager Manager { get; set; }

        /// <summary>
        /// The GameObject that owns this Component.
        /// </summary>
        /// 
        public GameObject Parent { get; set; }

        /// <summary>
        /// A new instance of Component.
        /// </summary>
        /// <param name="parent">The GameObject that owns this Component.</param>
        public Component(GameObject parent) 
        {
            Parent = parent;
        }

        /// <summary>
        /// Updates this Component.
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void HandleMessage(Message message)
        {

        }

    }
}
