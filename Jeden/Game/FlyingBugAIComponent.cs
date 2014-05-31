using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine.Object;
using SFML.Window;

namespace Jeden.Game
{
    class FlyingBugAIComponent : Component
    {
        Vector2f TargetPosition;
        Vector2f TargetDirection;
        Vector2f AreaPosition; // the area around where the bug will fly
        float AttackTime = 3;
        float AttackTimer;
        float NextAttack;

        Random Random;

        static int Seed = 0;
        public FlyingBugAIComponent(Vector2f areaPos, GameObject parent) : base(parent)
        {
            Random = new Random(Seed++);
            AreaPosition = areaPos;
            NextAttack = AttackTime;
        }

        float Dot(Vector2f a, Vector2f b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public override void Update(Engine.GameTime gameTime)
        {
            AttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(AttackTimer > NextAttack)
            {
                NextAttack += AttackTime;
                GameObjectFactory.CreateStinger(Parent, Parent.Position, new Vector2f(-1, 0));
            }

            base.Update(gameTime);

            if(Dot(Parent.Position - TargetPosition, TargetDirection) <= 0) // don't keep overshooting target
            {
                TargetPosition = AreaPosition + new Vector2f((float)Random.NextDouble() * 20 - 10, (float)Random.NextDouble() * 2 - 1);
                TargetDirection = TargetPosition - Parent.Position;
                Parent.HandleMessage(new FlyMessage(TargetDirection, Parent));
            }

            
        }
    }
}
