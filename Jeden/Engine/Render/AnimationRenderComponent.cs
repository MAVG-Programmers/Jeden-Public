using Jeden.Engine.Object;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace Jeden.Engine.Render
{

    class AnimationFinishedMessage : Message
    {
        public AnimationFinishedMessage(object sender) : base(sender) { }
    }
    /// <summary>
    /// RenderComponent for a looped animation sequence.
    /// </summary>
    
    public class AnimationRenderComponent : RenderComponent 
    {
        Animation Animation;

        public float FrameTime
        {
            get { return Animation.FrameTime; }
            set { Animation.FrameTime = value; }
        }

        public bool IsLooping
        {
            get { return Animation.IsLooping;  }
            set { Animation.IsLooping = value;  }
        }

        public bool IsFinished()
        {
            return Animation.IsFinished;
        }

        public AnimationRenderComponent(RenderManager renderMgr, GameObject parent)
            : base(renderMgr, parent) 
        {
            Animation = new Animation();
        }

        public AnimationRenderComponent(String filename, RenderManager renderMgr, GameObject parent) : base(renderMgr, parent)
        {
            Animation = new Animation(filename);
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

            WorldPosition = Parent.Position;
            Animation.Update(gameTime.ElapsedGameTime.TotalSeconds);

            if (Animation.IsFinished)
                Parent.HandleMessage(new AnimationFinishedMessage(this));

        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            Animation.Draw(renderMgr, WorldPosition, WorldWidth, WorldHeight, FlipX, FlipY, Tint, ZIndex);
        }


    }
}
