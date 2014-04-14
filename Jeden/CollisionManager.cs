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
                    GameObject gameObject1 = CollisionComponents[i].Parent;
                    GameObject gameObject2 = CollisionComponents[j].Parent;
                    if( !(gameObject1.Position.X + gameObject1.Width < gameObject2.Position.X ||
                        gameObject1.Position.Y < gameObject2.Position.Y + gameObject2.Height ||
                        gameObject1.Position.X > gameObject2.Position.X + gameObject2.Width ||
                        gameObject1.Position.Y + gameObject1.Height > gameObject2.Position.Y))
                    {
                        //Register collision
                        CollisionComponents[i].AddCollision(gameObject2);
                        CollisionComponents[j].AddCollision(gameObject1);
                    }
                }
            }
        }

    }
}
