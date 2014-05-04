using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Jeden.Engine.Object;
using SFML.Window;

namespace Jeden.Engine.Render
{
    /// <summary>
    /// Interface for rendering components.
    /// </summary>
    public class RenderComponent : Component 
    {
        public Vector2f Position { get; set; }
        public float WorldWidth { get; set; }
        public float WorldHeight { get; set; }
        public float Angle { get; set; }
        public Vector2f RotationCenter { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        public Color Tint { get; set; }
        public int ZIndex { get; set; }

        public virtual FloatRect GetScreenRect(Camera camera)
        {
            //TODO: DOES NOT WORK WITH ROTATIONS.

            FloatRect rect;
            rect.Width = WorldWidth;
            rect.Height = WorldHeight;
            rect.Top = Position.Y - WorldHeight * 0.5f;
            rect.Left = Position.X - WorldWidth * 0.5f;

            return rect;
        }

        public RenderComponent(GameObject parent) : base(parent)
        {
            Tint = new Color(255, 255, 255, 255);
        }
        
        public virtual void Draw(RenderManager renderMgr, Camera camera)
        {

        }

       
    }
}
