using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Render
{
    public class Renderer
    {
        public RenderTarget Target; //TODO, this is kind of a hack since each game state RenderManager

        public Renderer(RenderTarget target) 
        {
            Target = target;
            Renderables = new List<Renderable>();
        }

        public void ClearDrawList()
        {
            Renderables.Clear();
        }

        public void DrawSprite( Texture texture, 
                                IntRect subImageRect, 
                                Vector2f centerPos, 
								float viewWidth, 
                                float viewHeight, 
								float angle,
                                Vector2f rotationCenter,
								bool flipX, 
                                bool flipY, 
                                Color tint,
                                int zIndex)
        {
            Vertex[] vertices = new Vertex[4];

            float hw = viewWidth / 2.0f;
            float hh = viewHeight / 2.0f;

            vertices[0].Position = new Vector2f(-hw, +hh);
            vertices[1].Position = new Vector2f(-hw, -hh);
            vertices[2].Position = new Vector2f(+hw, -hh);
            vertices[3].Position = new Vector2f(+hw, +hh);

            if (flipX)
            {
                vertices[0].TexCoords.X = subImageRect.Left + subImageRect.Width;
                vertices[1].TexCoords.X = subImageRect.Left + subImageRect.Width;
                vertices[2].TexCoords.X = subImageRect.Left;
                vertices[3].TexCoords.X = subImageRect.Left;
            }
            else
            {
                vertices[0].TexCoords.X = subImageRect.Left;
                vertices[1].TexCoords.X = subImageRect.Left;
                vertices[2].TexCoords.X = subImageRect.Left + subImageRect.Width;
                vertices[3].TexCoords.X = subImageRect.Left + subImageRect.Width;
            }

            if (flipY)
            {
                vertices[0].TexCoords.Y = subImageRect.Top;
                vertices[1].TexCoords.Y = subImageRect.Top + subImageRect.Height;
                vertices[2].TexCoords.Y = subImageRect.Top + subImageRect.Height;
                vertices[3].TexCoords.Y = subImageRect.Top;
            }
            else
            {
                vertices[0].TexCoords.Y = subImageRect.Top + subImageRect.Height;
                vertices[1].TexCoords.Y = subImageRect.Top;
                vertices[2].TexCoords.Y = subImageRect.Top;
                vertices[3].TexCoords.Y = subImageRect.Top + subImageRect.Height;
            }

            Vector2f rotBasisX = new Vector2f((float)Math.Cos((double)angle), (float)Math.Sin((double)angle));
            Vector2f rotBasisY = new Vector2f(-rotBasisX.Y, rotBasisX.X);

            for (int i = 0; i < 4; i++)
            {
                vertices[i].Position.X = Vector2Dot(rotBasisX, vertices[i].Position - rotationCenter) + rotationCenter.X + centerPos.X;
                vertices[i].Position.Y = Vector2Dot(rotBasisY, vertices[i].Position - rotationCenter) + rotationCenter.Y + centerPos.Y;
            }

            vertices[0].Color = tint;
            vertices[1].Color = tint;
            vertices[2].Color = tint;
            vertices[3].Color = tint;

            RenderStates rs = new RenderStates();
            rs.BlendMode = BlendMode.Alpha;
            rs.Texture = texture;
            rs.Transform = Transform.Identity;
            rs.Shader = null;

            Renderable renderable;
            renderable.Vertices = vertices;
            renderable.RenderStates = rs;
            renderable.ZIndex = zIndex;

            Renderables.Add(renderable);
            //Target.Draw(vertices, PrimitiveType.Quads, rs);

        }

        public void Draw()
        {
            Renderables.Sort(new ZComparer());

            foreach(Renderable renderable in Renderables)
            {
                Target.Draw(renderable.Vertices, PrimitiveType.Quads, renderable.RenderStates);
            }
        }

        float Vector2Dot(Vector2f x, Vector2f y) // temporary, this need to be global
        {
            return x.X * y.X + x.Y * y.Y;
        }

        struct Renderable
        {


            public int ZIndex;
            public Vertex[] Vertices;
            public RenderStates RenderStates;
        }

        class ZComparer : IComparer<Renderable>
        {
            public int Compare(Renderable a, Renderable b)
            {
                if (a.ZIndex == b.ZIndex)
                    return 0;
                if (a.ZIndex < b.ZIndex)
                    return -1;
                return 1;
            }
        }

        List<Renderable> Renderables;

    }
}
