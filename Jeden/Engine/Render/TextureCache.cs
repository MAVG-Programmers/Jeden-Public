using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace Jeden.Engine.Render
{
    class TextureCache
    {
        static Dictionary<String, Texture> Textures = new Dictionary<string,Texture>();

        public static Texture GetTexture(String filename)
        {
            if (Textures.ContainsKey(filename))
                return Textures[filename];
            else
            {
                Texture texture = new Texture(filename);
                Textures.Add(filename, texture);
                return texture;
            }
        }


    }
}
