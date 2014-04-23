using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Object
{
    [Serializable]
    public class Component
    {
        public GameObject Parent { get; set; }

        public Component(GameObject parent) 
        {
            Parent = parent;
        }

        public virtual void Update(int dTime)
        {

        }
        //is a part of an entity
        //has property named "Parent", which represents the Entity it is a part of
        //serializable
    }
}
