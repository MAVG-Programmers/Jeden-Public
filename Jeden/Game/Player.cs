using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Engine.Render;
using SFML.Graphics;

namespace Jeden.Game
{
    class Player : GameObject
    {
        public Player(JedenGameState owner) : base(owner)
        {
            Texture texture = new Texture("assets/Test.png");
          //  Texture texture2 = new Texture("assets/Test2.png");

            SpriteRenderComponent src = owner.RenderMgr.MakeNewSpriteComponent(this, texture);

            AddComponent<HealthComponent>(owner.HealthMgr.MakeNewComponent(this, 100));
            AddComponent<RenderComponent>(src);
            Position.X = 400.0f;
            Position.Y = 300.0f;
        }

        public void ToggleSprite()
        {
            SpriteSetRenderComponent ssrc = (SpriteSetRenderComponent)GetComponent<RenderComponent>();
            
            if (ssrc.CurrentKey == "left")
                ssrc.SetCurrentSprite("right");
            else
                ssrc.SetCurrentSprite("left");

        }
    }
}
