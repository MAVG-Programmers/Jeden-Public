using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine.Object;

namespace Jeden.Game
{
    /// <summary>
    /// Sets the parent's .Valid = false after a set number of frames 
    /// </summary>
    class FrameLifetimeComponent : Component
    {
        public int Lifetime { get; set; }
        int Age { get; set; }

        public FrameLifetimeComponent(int frames, GameObject parent) : base(parent)
        {
            Lifetime = frames;
            Age = 0;
        }

        public override void Update(Engine.GameTime gameTime)
        {
            Age++;
            if(Age >= Lifetime)
            {
                Parent.HandleMessage(new InvalidateMessage(this));
            }
        }
    }
}
