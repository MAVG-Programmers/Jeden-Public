﻿
using System;
using System.Collections.Generic;


using SFML.Window;
using Jeden.Engine.Object;
using SFML.Graphics;


namespace Jeden.Engine.Render
{
    /// <summary>
    /// Internal animation class to share code between AnimationRenderComponent and AnimationSetRenderComponent.
    /// </summary>
    /// 
    class Animation 
    {
        /// <summary>
        /// The image part of a frame. 
        /// </summary>
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

        /// <summary>
        /// Adds a frame as the last frame of the animation.
        /// </summary>
        /// <param name="texture">The texture to draw for the fame.</param>
        /// <param name="subImageRect">The sub image rectange from the spriteshet</param>
        public void AddFrame(Texture texture, IntRect subImageRect)
        {
            SubImage subImage;
            subImage.Texture = texture;
            subImage.SubImageRect = subImageRect;

            Frames.Add(subImage);
        }

        public void AddFrame(Texture texture)
        {
            SubImage subImage;
            subImage.Texture = texture;
            subImage.SubImageRect.Left = 0;
            subImage.SubImageRect.Top = 0;
            subImage.SubImageRect.Width = (int) texture.Size.X;
            subImage.SubImageRect.Height = (int) texture.Size.Y;

            Frames.Add(subImage);
        }

        /// <summary>
        /// Draws the animation.
        /// </summary>
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

        /// <summary>
        /// Updates the animation[frame counter].
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            TimeAccum += deltaTime;

            if (TimeAccum > FrameTime)
            {
                TimeAccum -= FrameTime;
                CurrentFrame = (CurrentFrame + 1) % Frames.Count;
            }
        }

        /// <summary>
        /// Resets back to the first frame.
        /// </summary>
        public void Reset()
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
