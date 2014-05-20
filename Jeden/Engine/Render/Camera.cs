
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
    /// <summary>
    /// Camera, RenderComponents are drawn relative to the camera.
    /// </summary>
    public class Camera : View
    {
        public Camera()
        {

        }

        public void Update(GameTime gameTime)
        {
            if(Target != null)
                Center = Target.Position;
        }

        /// <summary>
        /// The clip rect of the screen. 
        /// No rotation yet...
        /// </summary>
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
       
        /// <summary>
        /// The GameObject that the camera follows.
        /// </summary>
        public GameObject Target { get; set; }
        
     
    }
}
