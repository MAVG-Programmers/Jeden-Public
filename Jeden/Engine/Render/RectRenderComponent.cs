using Jeden.Engine.Object;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeden.Engine.Render
{
    // Basically just a debug draw right now for collision shapes. Proably needs to be looked if you want to use for something else.
    public class RectRenderComponent : RenderComponent 
    {
        public static bool DebugDraw = true;

        RectangleShape RectShape;

        public RectRenderComponent(float width, float height, Color color, RenderManager renderMgr, GameObject parent) : base(renderMgr, parent)
        {
            WorldWidth = width;
            WorldHeight = height;
            RectShape = new RectangleShape(new SFML.Window.Vector2f(width, height));
            RectShape.FillColor = color;
            ZIndex = int.MaxValue - 10000;
        }

        public override void Update(GameTime gameTime)
        {
            WorldPosition = Parent.Position;
            base.Update(gameTime);
        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            if (DebugDraw)
            {
                WorldPosition = Parent.Position;
                RectShape.Position = Parent.Position - new Vector2f(RectShape.Size.X * 0.5f, RectShape.Size.Y * 0.5f);
                renderMgr.Target.Draw(RectShape);
            }
        }
    }
}
