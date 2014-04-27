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
            Texture texture = new Texture("assets/player.png");

            SpriteRenderComponent src = owner.RenderMgr.MakeNewSpriteComponent(this, texture);

            AddComponent<RenderComponent>(src);

            AddComponent<HealthComponent>(owner.HealthMgr.MakeNewComponent(this, 100));

            AddComponent<PhysicsComponent>(owner.PhysicsMgr.MakeNewComponent(this, 300.0f, 50.0f, 10.0f, 10.0f, true));

        }
    }
}
