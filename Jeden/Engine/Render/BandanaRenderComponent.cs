
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using Jeden.Engine.Object;

namespace Jeden.Engine.Render
{
    class Particle
    {
        public Vector2f x0;
        public Vector2f x;
        public Vector2f v;
        public Vector2f f;
        public float m;
    }


    class DistanceJoint
    {
        public Particle p0, p1;
        public float len;

        public static float Dot(Vector2f x, Vector2f y) // temporary, this need to be global
        {
            return x.X * y.X + x.Y * y.Y;
        }

        public static float Length(Vector2f v)
        {
            return (float)Math.Sqrt(Dot(v, v));
        }

        public void Solve()
        {
            Vector2f dx = p1.x - p0.x;
            float dxLen = Length(dx);
            Vector2f dir;
            if (dxLen > 0.000001f)
            {
                dir = dx / dxLen;
            }
            else
            {
                dir = new Vector2f(0.0f, -1.0f);
            }
            float dif = dxLen - len;

            float slopPercent = 0.1f;
            if (Math.Abs(dif) > slopPercent * len)
            {
                float slop = dif > 0.0f ? slopPercent * len : -slopPercent * len;
                Vector2f P = (dir / 2.0f) * (dif - slop);
                p0.x += P;
                p1.x -= P;
            }
        }
    }

    struct Cloth
    {
        public const int nParticlesX = 11;
        public const int nParticlesY = 9;
        public const int nParticles = nParticlesX * nParticlesY;
        public const float patchSize = 10.0f;
        public const int numIterations = 3;

        public void Init()
        {
            particles = new Particle[nParticles];
            for (int i = 0; i < nParticles; i++)
                particles[i] = new Particle();
            
            joints = new List<DistanceJoint>();
           

            for (int i = 0; i < nParticlesX; i++)
            {
                for (int j = 0; j < nParticlesY; j++)
                {
                    particles[i + j * nParticlesX].x = new Vector2f((float)(i), (float)(-j)) * patchSize;
                    particles[i + j * nParticlesX].x0 = new Vector2f((float)(i), (float)(-j)) * patchSize;
                    particles[i + j * nParticlesX].v = new Vector2f(0.0f, 0.0f);
                }
            }

            for (int y = 0; y < nParticlesY; y++)
            {
                for (int x = 0; x < nParticlesX - 1; x++)
                {
                    DistanceJoint joint = new DistanceJoint();
                    joint.p0 = particles[x + y * nParticlesX];
                    joint.p1 = particles[x + 1 + y * nParticlesX];
                    joint.len = 10;
                    joints.Add(joint);
                }
            }
            for (int x = 0; x < nParticlesX; x++)
            {
                for (int y = 0; y < nParticlesY - 1; y++)
                {
                    DistanceJoint joint = new DistanceJoint();
                    joint.p0 = particles[x + y * nParticlesX];
                    joint.p1 = particles[x + (y + 1) * nParticlesX];
                    joint.len = 10;
                    joints.Add(joint);
                }
            }

            /*
            for (int i = 0; i < nParticles; i++)
            {
                if ((i + 1) % nParticlesX != 0 && i != nParticles - 1)
                {
                    joints[joint].p0 = particles[i];
                    joints[joint].p1 = particles[i + 1];
                    joints[joint].len = patchSize;
                    joint++;
                }
                if (i < nParticles - nParticlesX)
                {
                    joints[joint].len = patchSize;
                    joints[joint].p0 = particles[i];
                    joints[joint].p1 = particles[i + nParticlesY];
                    joint++;
                }
                if (((i + 1) % nParticlesX != 0 && i != nParticles - 1) && (i < nParticles - nParticlesX))
                {
                    joints[joint].len = (float)Math.Sqrt(2.0f * patchSize * patchSize);
                    joints[joint].p0 = particles[i];
                    joints[joint].p1 = particles[i + 1 + nParticlesY];
                    joint++;
                }
                if ((i % nParticlesX != 0 && i != 0) && (i < nParticles - nParticlesX))
                {
                    joints[joint].len = (float)Math.Sqrt(2.0f * patchSize * patchSize);
                    joints[joint].p0 = particles[i];
                    joints[joint].p1 = particles[i - 1 + nParticlesY];
                    joint++;
                }
            }
             */
        }

        public void Step(Vector2f mouse, float dt)
        {
            for (int i = 0; i < joints.Count; i++)
            {
                //	joints[i].AddForce();
            }
            for (int i = 0; i < nParticles; i++)
            {
                Vector2f tmp = particles[i].x;
                particles[i].x += 0.98f * (particles[i].x - particles[i].x0) + new Vector2f(0, dt);  //TODO: this gravity is a hack
                particles[i].x0 = tmp;
                particles[i].f = new Vector2f(0.0f, 0.0f);
            }
            
            for (int k = 0; k < numIterations; k++)
            {
                particles[0].x = mouse;
                particles[nParticlesX - 1].x = mouse + new Vector2f(20, 10);
                for (int q = 0; q < joints.Count; q++)
                {
                    joints[q].Solve();
                }
            }
             
        }
        public Particle[] particles;
        public List<DistanceJoint> joints;
    }


    class BandanaRenderComponent : RenderComponent
    {
        GameObject Target;

        public BandanaRenderComponent(GameObject target, GameObject parent)
            : base(parent)
        {
            cloth = new Cloth();
            cloth.Init();
            Target = target;
           
        }
        public static float Vector2Dot(Vector2f x, Vector2f y) // temporary, this need to be global
        {
            return x.X * y.X + x.Y * y.Y;
        }

        public static float Vector2Length(Vector2f v)
        {
            return (float)Math.Sqrt(Vector2Dot(v, v));
        }

        Cloth cloth;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            cloth.Step(Target.Position, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw(RenderManager renderMgr, Camera camera)
        {
            base.Draw(renderMgr, camera);


            for (int i = 0; i < Cloth.nParticlesX - 1; i++)
            {
                for (int j = 0; j < Cloth.nParticlesY - 1; j++)
                {
                    SFML.Graphics.Vertex[] verts = new SFML.Graphics.Vertex[4];
                    verts[0].Position = cloth.particles[i + Cloth.nParticlesX * j].x;
                    verts[1].Position = cloth.particles[(i + 1) + Cloth.nParticlesX * j].x;
                    verts[2].Position = cloth.particles[(i + 1) + Cloth.nParticlesX * (j + 1)].x;
                    verts[3].Position = cloth.particles[i + Cloth.nParticlesX * (j + 1)].x;

                    verts[0].Color = new SFML.Graphics.Color((byte)i, 255, 255, 255);
                    verts[1].Color = new SFML.Graphics.Color((byte)j, 255, 255, 255);
                    verts[2].Color = new SFML.Graphics.Color(255, 255, 0, 255);
                    verts[3].Color = new SFML.Graphics.Color(255, 0, 255, 255);

                    verts[0].TexCoords = new Vector2f((float)((float)(i) / (float)(Cloth.nParticlesX - 1)), (float)((float)(j) / (float)(Cloth.nParticlesY - 1)));
                    verts[1].TexCoords = new Vector2f((float)((float)(i + 1) / (float)(Cloth.nParticlesX - 1)), (float)((float)(j) / (float)(Cloth.nParticlesY - 1)));
                    verts[2].TexCoords = new Vector2f((float)((float)(i + 1) / (float)(Cloth.nParticlesX - 1)), (float)((float)(j + 1) / (float)(Cloth.nParticlesY - 1)));
                    verts[3].TexCoords = new Vector2f((float)(i) / (float)(Cloth.nParticlesX - 1), (float)(j + 1) / (float)(Cloth.nParticlesY - 1));

                    renderMgr.Target.Draw(verts, SFML.Graphics.PrimitiveType.Quads);
                }
            }
        }
    }
}
