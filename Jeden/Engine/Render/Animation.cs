
using System;
using System.Collections.Generic;


using SFML.Window;
using Jeden.Engine.Object;
using SFML.Graphics;


namespace Jeden.Engine.Render
{

    class Animation 
    {
        public struct SubImage
        {
            public Texture Texture;
            public IntRect SubImageRect;
        }


        public Animation()
        {
            FrameTime = 0;
            CurrentFrame = 0;
            TimeAccum = 0.0f;
            Frames = new List<SubImage>();
        }

        public void AddFrame(Texture texture, IntRect subImageRect)
        {
            SubImage subImage;
            subImage.Texture = texture;
            subImage.SubImageRect = subImageRect;

            Frames.Add(subImage);
        }

        public void Draw(Renderer renderer, 
                                Vector2f centerPos,
                                float viewWidth,
                                float viewHeight,
                                float angle,
                                Vector2f rotationCenter,
                                bool flipX,
                                bool flipY,
                                Color tint)
        {
            renderer.DrawSprite(Frames[CurrentFrame].Texture, Frames[CurrentFrame].SubImageRect,
                centerPos, viewWidth, viewHeight, angle, rotationCenter, flipX, flipY, tint);
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
        List<SubImage> Frames;
    }
}
