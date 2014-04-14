using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    public class CollisionManager
    {
        List<CollisionComponent> CollisionComponents;

        CollisionManager()
        {
            CollisionComponents = new List<CollisionComponent>();
        }

        public void Add(CollisionComponent cc)
        {
            if (!CollisionComponents.Contains(cc))
            {
                CollisionComponents.Add(cc);
            }
        }

        public void Remove(CollisionComponent cc)
        {
            if (CollisionComponents.Contains(cc))
            {
                CollisionComponents.Remove(cc);
            }
        }

        public void Detect()
        {
            for (int i = 0; i < CollisionComponents.Count; i++)
            {
                for (int j = i; j < CollisionComponents.Count; j++)
                {
                    GameObject o1 = CollisionComponents[i].Parent;
                    GameObject o2 = CollisionComponents[j].Parent;
                    if( !(o1.x+o1.width < o2.x ||
                        o1.y < o2.y+o2.height ||
                        o1.x > o2.x+o2.width ||
                        o1.y + o1.height > o2.y))
                    {
                        //Register collision
                        CollisionComponents[i].AddCollision(o2);
                        CollisionComponents[j].AddCollision(o1);
                    }
                }
            }
        }

    }
}
