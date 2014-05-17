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
    //Wrapper around SFML sprites as components
    public class SpriteRenderComponent : RenderComponent
    {
        protected Texture Texture;
        protected IntRect SubImageRect;

        public SpriteRenderComponent(RenderManager renderMgr, GameObject parent, Texture texture)
            : base(renderMgr, parent)
        
        {
            Texture = texture;
            Position = parent.Position;
            SubImageRect.Top = 0;
            SubImageRect.Left = 0;
            SubImageRect.Width = (int)texture.Size.X;
            SubImageRect.Height = (int)texture.Size.Y;
            WorldWidth = SubImageRect.Width;
            WorldHeight = SubImageRect.Height;
            FlipX = false;
            FlipY = false;
            Tint = new Color(255, 255, 255, 255);
        }

        public SpriteRenderComponent(RenderManager renderMgr, GameObject parent, Texture texture, IntRect subImageRect)
            : base(renderMgr, parent)
        {
            Texture = texture;
            Position = parent.Position;
            SubImageRect = subImageRect;
            WorldWidth = SubImageRect.Width;
            WorldHeight = SubImageRect.Height;
            FlipX = false;
            FlipY = false;
            Tint = new Color(255, 255, 255, 255);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Update position from parent
            Position = Parent.Position;
        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            renderMgr.DrawSprite(Texture, SubImageRect, Position, WorldWidth, WorldHeight, FlipX, FlipY, Tint, ZIndex);
        }
    }
}
