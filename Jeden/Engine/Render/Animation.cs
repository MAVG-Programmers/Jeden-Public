
using System;
using System.Collections.Generic;


using SFML.Window;
using Jeden.Engine.Object;
using SFML.Graphics;


namespace Jeden.Engine.Render
{

    class Animation 
    {
        public struct Frame
        {
            public Texture Texture;
            public IntRect SubImageRect;
        }


        Animation()
        {
            FrameTime = 0;
            CurrentFrame = 0;
            TimeAccum = 0.0f;
        }


        public void Draw(RenderTarget target, Vector2f position)
        {
            RenderStates rs;
            rs.BlendMode = BlendMode.Alpha;
            rs.Texture = Frames[CurrentFrame].Texture;
            rs.Transform = new Transform();
            rs.Transform.Translate(position);
        }

        public void Update(float deltaTime)
        {
            TimeAccum += deltaTime;

            if (TimeAccum > FrameTime)
            {
                TimeAccum -= FrameTime;
                CurrentFrame = (CurrentFrame + 1) % Frames.Count;
            }
        }


        void Reset()
        {
            TimeAccum = 0.0f;
            CurrentFrame = 0;
        }

        int CurrentFrame;
        float TimeAccum;
        float FrameTime;
        List<Frame> Frames;
    }
}
