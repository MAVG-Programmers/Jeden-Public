using Jeden.Engine.Object;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Render
{
    /// <summary>
    /// RenderComponent for a looped animation sequence.
    /// </summary>
    public class AnimationRenderComponent : RenderComponent 
    {
        Animation Animation;

        public AnimationRenderComponent(GameObject parent) : base(parent) 
        {
            Animation = new Animation();
        }

        /// <summary>
        /// Adds a frame to the end of the animation sequence.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="subImageRect"></param>
        public void AddFrame(Texture texture, IntRect subImageRect)
        {
            Animation.AddFrame(texture, subImageRect);
        }

        /// <summary>
        /// Adds a frame to the end of the animation sequence.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="subImageRect"></param>
        public void AddFrame(Texture texture)
        {
            Animation.AddFrame(texture);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position = Parent.Position;
            Animation.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        }

        public override void Draw(Renderer renderer)
        {
            Animation.Draw(renderer, Position, WorldWidth, WorldHeight, Angle, RotationCenter, FlipX, FlipY, Tint);
        }


    }
}
