using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Vector2f WorldPosition;

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        public float WorldWidth { get; set; }
        public float WorldHeight { get; set; }
        public Color Tint { get; set; }
        public int ZIndex { get; set; }

        public bool AlwaysVisible;

        public virtual FloatRect GetScreenRect(Camera camera)
        {
            
            FloatRect rect = new FloatRect();
            
            rect.Width = WorldWidth;
            rect.Height = WorldHeight;
            rect.Top = WorldPosition.Y - WorldHeight * 0.5f;
            rect.Left = WorldPosition.X - WorldWidth * 0.5f;
            
            /*
            rect.Width = float.PositiveInfinity;
            rect.Width = float.PositiveInfinity;
            rect.Top = 0;
            rect.Left = 0;
            */
            return rect;
        }

        public RenderComponent(RenderManager renderMgr, GameObject parent) : base(parent)
        {
            Manager = renderMgr;

            Tint = new Color(255, 255, 255, 255);
        }
        
        public virtual void Draw(RenderManager renderMgr, Camera camera)
        {

        }

       
    }
}
