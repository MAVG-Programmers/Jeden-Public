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
            AddComponent<HealthComponent>(owner.HealthMgr.MakeNewComponent(this, 100));
            AddComponent<RenderComponent>((RenderComponent)owner.RenderMgr.MakeNewComponent(this, texture));
            Position.X = 100.0f;
            Position.Y = 10.0f;
        }
    }
}
