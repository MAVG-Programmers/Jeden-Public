using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine.Object;
using Jeden.Engine.Render;
using Jeden.Game.Physics;
using SFML.Graphics;

namespace Jeden.Game
{
    class Player : GameObject
    {
        public Player(JedenGameState owner) : base(owner)
        {
            Texture texture = new Texture("assets/Test.png");

            SpriteRenderComponent src = owner.RenderMgr.MakeNewSpriteComponent(this, texture);

            AddComponent<HealthComponent>(owner.HealthMgr.MakeNewComponent(this, 100));

            Position.X = 100.0f;
            Position.Y = 10.0f;
            AddComponent<PhysicsComponent>(owner.PhysicsMgr.MakeNewComponent(this, 100.0f, 50.0f, 10.0f, 10.0f, true));

        }
    }
}
