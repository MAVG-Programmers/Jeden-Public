﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Jeden.Engine.Object;
using SFML.Window;

namespace Jeden.Engine.Render
{
    //Wrapper around SFML sprites as components
    public class RenderComponent : Component 
    {
        public Vector2f Position { get; set; }
        public float ViewWidth { get; set; }
        public float ViewHeight { get; set; }
        public float Angle { get; set; }
        public Vector2f RotationCenter { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        public Color Tint { get; set; }

        public RenderComponent(GameObject parent) : base(parent) { }
        
        public virtual void Draw(Renderer renderer)
        {

        }
    }
}
