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
            Texture texture2 = new Texture("assets/test.png");

            AnimationRenderComponent arc = owner.RenderMgr.MakeNewAnimationComponent(this);
            arc.AddFrame(texture);
            arc.AddFrame(texture2);
            arc.WorldWidth = 64;
            arc.WorldHeight = 64;

            AddComponent<RenderComponent>(arc);

            AddComponent<HealthComponent>(owner.HealthMgr.MakeNewComponent(this, 100));

            AddComponent<PhysicsComponent>(owner.PhysicsMgr.MakeNewComponent(this, 10.0f, 10.0f, true));

        }
    }
}
