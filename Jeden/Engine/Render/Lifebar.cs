using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeden.Engine.Object;
using SFML.Graphics;
using SFML.Window;

namespace Jeden.Engine.Render
{
    class Lifebar : RenderComponent 
    {
        Vector2f Offset; // from top left corner of screen

        Texture BorderTexture;
        Vertex[] BorderVertices;


        Texture HealthTexture;
        Vertex[] HealthVertices;

        Texture SheildTexture;
        Vertex[] SheildVertices;

        

        public Lifebar(RenderManager renderMgr, GameObject parent)
            : base(renderMgr, parent)
        {
            AlwaysVisible = true;

            BorderVertices = new Vertex[4];
            HealthVertices = new Vertex[4];
            SheildVertices = new Vertex[4];

            Offset = new Vector2f(30, 20);

            for (int i = 0; i < 4; i++)
            {
                BorderVertices[i] = new Vertex();
                BorderVertices[i].Color = new Color(255, 255, 255, 255);
            }

            BorderTexture = new Texture("assets/lifebar.png");

            BorderVertices[0].Position = new Vector2f(0, 0);
            BorderVertices[1].Position = new Vector2f(BorderTexture.Size.X, 0);
            BorderVertices[2].Position = new Vector2f(BorderTexture.Size.X, BorderTexture.Size.Y);
            BorderVertices[3].Position = new Vector2f(0, BorderTexture.Size.Y);

            for (int i = 0; i < 4; i++)
            {
                BorderVertices[i].Position = BorderVertices[i].Position + Offset;
            }

            BorderVertices[0].TexCoords = new Vector2f(0, 0);
            BorderVertices[1].TexCoords = new Vector2f(BorderTexture.Size.X, 0);
            BorderVertices[2].TexCoords = new Vector2f(BorderTexture.Size.X, BorderTexture.Size.Y);
            BorderVertices[3].TexCoords = new Vector2f(0, BorderTexture.Size.Y);

        }
        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            renderMgr.SetOverlayView();

            RenderStates renderStates = new RenderStates(BorderTexture);

            renderMgr.Target.Draw(BorderVertices, PrimitiveType.Quads, renderStates);

            renderMgr.SetCameraView();
        }
    }
}
