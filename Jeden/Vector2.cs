using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Jeden
{
    [Serializable]
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Length 
        {
            get 
            {
                return (float) Math.Sqrt(X * X + Y * Y);
            }
        }

        public static Vector2 Zero 
        {
            get 
            {
                return new Vector2(0, 0);
            }
        }
        public static Vector2 UnitX 
        {
            get 
            {
                return new Vector2(1, 0);
            }
        }
        public static Vector2 UnitY
        {
            get
            {
                return new Vector2(0, 1);
            }
        }
        public static Vector2 One
        {
            get
            {
                return new Vector2(1, 1);
            }
        }

        public Vector2() : this(0,0)
        {
        }
        public Vector2(float x, float y) 
        {
            X = x;
            Y = y;
        }

        public void Normalize() 
        {
            X = X / Length;
            Y = Y / Length;
        }
        public Vector2f ToSFMLVector2f() 
        {
            return new Vector2f(X, Y);
        }
        public Vector2i ToSFMLVector2i() 
        {
            return new Vector2i((int) X, (int) Y);
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(right.X - left.X, right.Y + left.Y);
        }
        public static Vector2 operator -(Vector2 vector) 
        {
            return new Vector2(-vector.X, -vector.Y);
        }
        public static float operator *(Vector2 left, Vector2 right) 
        {
            return ((left.X * right.X) + (left.Y * right.Y));
        }
        public static Vector2 operator *(float multiplier, Vector2 vector) 
        {
            return new Vector2(vector.X * multiplier, vector.Y * multiplier);
        }
    }
}
