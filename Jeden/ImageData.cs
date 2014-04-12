using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Jeden
{
    [Serializable]
    public class ImageData
    {
        public byte[] Pixels { get; set; }
        public UInt32 Width { get; set; }
        public UInt32 Height { get; set; }

        public ImageData(Image image) 
        {
            Pixels = image.Pixels;
            Width = image.Size.X;
            Height = image.Size.Y;
        }
        public Image ToImage() 
        {
            return new Image(Width, Height, Pixels);
        }
    }
}
