
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jeden.Engine.Object;
using SFML.Graphics;

namespace Jeden.Engine.Render
{
    public class AnimationSetRenderComponent : RenderComponent 
    {
        Dictionary<String, Animation> AnimationSet;
        Animation CurrentAnimation;
        
        public String CurrentKey { get; private set; }


        public AnimationSetRenderComponent(GameObject parent) : base(parent)
        {
            AnimationSet = new Dictionary<string, Animation>();
        }

        public void SetFrameTime(String key, float frameTime)
        {
            Animation anim;
            if (AnimationSet.TryGetValue(key, out anim))
            {
                anim.FrameTime = frameTime;
            }
            else
            {
                //TODO: Throw and exception or something...
            }
        }
        public void AddFrame(String key, Texture texture, IntRect subImageRect)
        {
            Animation anim;
            if(AnimationSet.TryGetValue(key, out anim))
            {
                anim.AddFrame(texture, subImageRect);
            }
            else
            {
                anim = new Animation();
                anim.AddFrame(texture, subImageRect);
                AnimationSet.Add(key, anim);
            }
        }

        public void SetAnimation(String key)
        {   
            Animation anim;

            if (key == CurrentKey)
                return;

            if (AnimationSet.TryGetValue(key, out anim))
            {
                CurrentKey = key;
                CurrentAnimation.Reset();
                CurrentAnimation = anim;
            }
            else
            {
                //TODO: thow an exception or something...
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position = Parent.Position;
            CurrentAnimation.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            CurrentAnimation.Draw(renderMgr, Position, WorldWidth, WorldHeight, Angle, RotationCenter, FlipX, FlipY, Tint, ZIndex);
        }
    }
}
