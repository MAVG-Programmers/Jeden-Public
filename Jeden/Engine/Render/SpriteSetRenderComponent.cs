using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Jeden.Engine.Object;

namespace Jeden.Engine.Render
{
    public class SpriteSetRenderComponent : RenderComponent 
    {

        struct SubImage
        {
            public Texture Texture;
            public IntRect SubImageRect;
        }
        Dictionary<String, SubImage> SpriteSet;
        SubImage CurrentSprite;

        public String CurrentKey;

        public SpriteSetRenderComponent(RenderManager renderMgr, GameObject parent)
            : base(renderMgr, parent)
        {
            SpriteSet = new Dictionary<string, SubImage>();
        }

        public void AddSprite(String key, Texture texture)
        {
            SubImage subImage;
            subImage.Texture = texture;
            subImage.SubImageRect.Left = 0;
            subImage.SubImageRect.Top = 0;
            subImage.SubImageRect.Width = (int)texture.Size.X;
            subImage.SubImageRect.Height = (int)texture.Size.Y;

            SpriteSet.Add(key, subImage);
        }

        public void SetCurrentSprite(String key)
        {
            if (key == CurrentKey)
                return;

            SubImage sprite;
            if (SpriteSet.TryGetValue(key, out sprite))
            {
                CurrentSprite = sprite;
                CurrentKey = key;
            }
            else
            {
                //TODO: Throw exception or something...
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Update position from parent
            WorldPosition = Parent.Position;
        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            renderMgr.DrawSprite(
                CurrentSprite.Texture, 
                CurrentSprite.SubImageRect, 
                WorldPosition, 
                WorldWidth, 
                WorldHeight,
                FlipX, 
                FlipY,
                Tint, ZIndex);
        }
    }
}
