using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Jeden.Engine.Object;

namespace Jeden.Engine.Render
{
    //Wrapper around SFML sprites as components
    public class RenderComponent : Component
    {
        public Sprite Sprite { get; set; }

        public RenderComponent(GameObject parent, Texture texture) : base(parent)
        {
            Sprite = new Sprite(texture);
            Sprite.Position = Parent.Position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Update position from parent
            Sprite.Position = Parent.Position;
        }
    }
}
