using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    public class CollisionComponent : Component
    {
        public List<GameObject> CollidingGameObjects { get; set; }

        public CollisionComponent(GameObject parent) : base(parent)
        {

        }

        public void AddCollision(GameObject obj)
        {
            if (!CollidingGameObjects.Contains(obj))
            {
                CollidingGameObjects.Add(obj);
            }
        }

        public void ClearCollisions()
        {
            CollidingGameObjects.Clear();
        }

        public override void Update(JedenGame game)
        {

        }
        public override void Draw(JedenGame game) 
        {
            //It might be useful for debugging to allow bounding boxes to draw
        }
    }
}
