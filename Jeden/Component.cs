using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    [Serializable]
    public class Component
    {
        public GameObject Parent { get; set; }

        public Component(GameObject parent) 
        {
            Parent = parent;
        }

        public virtual void LoadContent(JedenGame game)
        {

        }
        public virtual void Update(JedenGame game)
        {

        }
        public virtual void Draw(JedenGame game)
        {

        }
        public virtual void UnloadContent(JedenGame game)
        {

        }

        //is a part of an entity
        //has property named "Parent", which represents the Entity it is a part of
        //serializable
    }
}
