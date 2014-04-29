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

            Target.Draw(vertices, PrimitiveType.Quads, rs);

        }

        /*
                 public void DrawCylinder(Vector2 A, Vector2 B, float radius, Color4 fillColor, Color4 outlineColor)
        {


            Vector2 AB = B - A;
            float ang0 = (float)System.Math.Atan2(AB.Y, AB.X) + (float)MathFunctions.PI / 2.0f;

            const float N = 8;

            if (fillColor.A > 0.0f)
            {
                SetColor(fillColor);
                GL.Begin(PrimitiveType.TriangleFan);
                for (int n = 0; n <= N; n++)
                {
                    float f = ang0 + ((float)n / (float)N) * (float)MathFunctions.PI;
                    Vector2 v = A + radius * new Vector2((float)MathFunctions.Cos(f), (float)MathFunctions.Sin(f));
                    GL.Vertex2(v.X, v.Y);
                }
                for (int n = 0; n <= N; n++)
                {
                    float f = ang0 + (float)MathFunctions.PI + ((float)n / (float)N) * (float)MathFunctions.PI;
                    Vector2 v = B + radius * new Vector2((float)MathFunctions.Cos(f), (float)MathFunctions.Sin(f));
                    GL.Vertex2(v.X, v.Y);
                }
                GL.End();


            }

            if (outlineColor.A > 0.0f)
            {
                SetColor(outlineColor);
                GL.Begin(PrimitiveType.LineLoop);
                for (int n = 0; n <= N; n++)
                {
                    float f = ang0 + ((float)n / (float)N) * (float)MathFunctions.PI;
                    Vector2 v = A + radius * new Vector2((float)MathFunctions.Cos(f), (float)MathFunctions.Sin(f));
                    GL.Vertex2(v.X, v.Y);
                }
                for (int n = 0; n <= N; n++)
                {
                    float f = ang0 + (float)MathFunctions.PI + ((float)n / (float)N) * (float)MathFunctions.PI;
                    Vector2 v = B + radius * new Vector2((float)MathFunctions.Cos(f), (float)MathFunctions.Sin(f));
                    GL.Vertex2(v.X, v.Y);
                }
                GL.End();
            }



        }

        public void DrawBox(Box2 box, Color4 fillColor, Color4 outlineColor)
        {
            if (fillColor.A > 0.0f)
            {
                SetColor(fillColor);
                GL.Begin(PrimitiveType.Quads);

                GL.Vertex2(box.Left, box.Bottom);
                GL.Vertex2(box.Right, box.Bottom);
                GL.Vertex2(box.Right, box.Top);
                GL.Vertex2(box.Left, box.Top);

                GL.End();
            }

            if (outlineColor.A > 0.0f)
            {
                SetColor(outlineColor);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(box.Left, box.Bottom);
                GL.Vertex2(box.Right, box.Bottom);
                GL.Vertex2(box.Right, box.Top);
                GL.Vertex2(box.Left, box.Top);

                GL.End();
            }
        }
        */
        public void DrawCircle(Vector2f center, float radius, Color fillColor, Color outlineColor)
        {
            const int NumCircleVerts = 16;
            Vertex[] vertices = new Vertex[16];

            for (int i = 0; i < NumCircleVerts; i++)
            {
                Vector2f v = center + radius * new Vector2f(
                    (float)Math.Cos(2.0f * Math.PI * (float)i / (float)NumCircleVerts),
                    (float)Math.Sin(2.0f * Math.PI * (float)i / (float)NumCircleVerts));

                vertices[i].Position = v;
                vertices[i].Color = fillColor;

            }
            Target.Draw(vertices, PrimitiveType.TrianglesFan);

            for (int i = 0; i < NumCircleVerts; i++)
                vertices[i].Color = outlineColor;
            Target.Draw(vertices, PrimitiveType.LinesStrip);
        }
        
        public void DrawLine(Vector2f A, Vector2f B, Color color)
        {
            Vertex[] vertices = new Vertex[2];
            vertices[0].Color = color;
            vertices[1].Color = color;
            vertices[0].Position = A;
            vertices[1].Position = B;
            Target.Draw(vertices, PrimitiveType.Lines);
        }
         


        float Vector2Dot(Vector2f x, Vector2f y) // temporary, this need to be global
        {
            return x.X * y.X + x.Y * y.Y;
        }

    }
}
