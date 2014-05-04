
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Jeden.Engine.Object;

namespace Jeden.Engine.Render
{
    public class Camera : View
    {
        public Camera()
        {
            Size = new Vector2f(800, 600);
        }

        public void Update(GameTime gameTime)
        {
            if(Target != null)
                Center = Target.Position;
        }

        public FloatRect ViewRect
        {
            get
            {
                FloatRect rect;
                rect.Left = Center.X - Size.X * 0.5f;
                rect.Top = Center.Y - Size.Y * 0.5f;
                rect.Width = Size.X;
                rect.Height = Size.Y;
                return rect;
            }
        }
       

        public GameObject Target { get; set; }
        
     
    }
}
