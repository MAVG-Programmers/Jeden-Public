using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Jeden
{
    [Serializable]
    public class RectangleCollider
    {
        public Vector2i Position { get; set; }
        public GameObject Parent { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Intersects(RectangleCollider rectangleCollider) 
        {
            if (!(rectangleCollider.Position.X + rectangleCollider.Width < this.Position.X ||
                        rectangleCollider.Position.Y < this.Position.Y + this.Height ||
                        rectangleCollider.Position.X > this.Position.X + this.Width ||
                        rectangleCollider.Position.Y + rectangleCollider.Height > this.Position.Y))
            {
                return true;
            }
            return false;
        }
        public void Draw(JedenGame game) 
        {
            //for debug purposes
            //maybe even for the level editor to show where the collisions are?
        }
    }
}
