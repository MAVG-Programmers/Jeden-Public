
using System;
using System.Collections.Generic;


using SFML.Window;
using Jeden.Engine.Object;
using SFML.Graphics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;


namespace Jeden.Engine.Render
{
    /// <summary>
    /// Internal animation class to share code between AnimationRenderComponent and AnimationSetRenderComponent.
    /// </summary>
    /// 
    [DataContract]
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
            Frames = new List<SubImage>();
            IsLooping = true;
        }

        public float FrameTime { get; set; }
        public bool IsLooping { get; set; }
        public bool IsFinished { get; private set; } // set to true after the last frame is drawn for FrameTime of a non looping animation

        List<SubImage> Frames;

        int CurrentFrame;
        double NextUpdate;
        double Time;


        /// <summary>
        /// Adds a frame as the last frame of the animation.
        /// </summary>
        /// <param name="texture">The texture to draw for the fame.</param>
        /// <param name="subImageRect">The sub image rectangle from the spriteshet</param>
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
        public void Draw(RenderManager renderMgr, 
                                Vector2f centerPos,
                                float viewWidth,
                                float viewHeight,
                                bool flipX,
                                bool flipY,
                                Color tint,
                                int zIndex)
        {
            renderMgr.DrawSprite(Frames[CurrentFrame].Texture, Frames[CurrentFrame].SubImageRect,
                centerPos, viewWidth, viewHeight, flipX, flipY, tint, zIndex);
        }

        /// <summary>
        /// Updates the animation[frame counter].
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime)
        {
            Time += deltaTime;

            if (Time > NextUpdate)
            {
                if (IsLooping)
                {
                    NextUpdate += FrameTime;
                    CurrentFrame = (CurrentFrame + 1) % Frames.Count;
                }
                else
                { 
                    // If not looping just stop on the last frame.
                    NextUpdate += FrameTime;
                    CurrentFrame = (CurrentFrame + 1);
                    if (CurrentFrame > Frames.Count - 1)
                    {
                        CurrentFrame = Frames.Count - 1;
                        NextUpdate = float.PositiveInfinity;
                        IsFinished = true;
                    }
                }
            }
        }

        /// <summary>
        /// Resets back to the first frame.
        /// </summary>
        public void Reset()
        {
            NextUpdate = Time + FrameTime;
            CurrentFrame = 0;
            IsFinished = false;
        }

    }
}
