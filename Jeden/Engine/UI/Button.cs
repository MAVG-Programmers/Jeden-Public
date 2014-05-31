
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Jeden.Engine.Render;
using SFML.Window;

namespace Jeden.Engine.UI
{
    class Button
    {
        Texture texture;
        FloatRect box;

        public Button(Texture tex, FloatRect b)
        {
            texture = tex;
            box = b;
            press = false;
        }

        public void Update(InputManager InputMgr, float deltaTime)
        {
            if (InputMgr.IsButtonDown(SFML.Window.Mouse.Button.Left))
                if (box.Contains(InputMgr.MousePosition.X, InputMgr.MousePosition.Y))
                {
                    press = true;
                }
            if (InputMgr.IsButtonDown(SFML.Window.Mouse.Button.Left) == false && press == true)
                if (box.Contains(InputMgr.MousePosition.X, InputMgr.MousePosition.Y))
                {
                    Clicked(this, null);
                    press = false;
                }
            if (box.Contains(InputMgr.MousePosition.X, InputMgr.MousePosition.Y) == false)
            {
                press = false;
            }
        }

        public void Draw(RenderManager renderMgr)
        {
            IntRect subImageRect = new IntRect(0, 0, (int) texture.Size.X, (int) texture.Size.Y);
            Vector2f centerPos = new Vector2f(box.Left + box.Width * 0.5f, box.Top + box.Height * 0.5f);

            renderMgr.DrawSprite(texture, subImageRect, centerPos, box.Width, box.Height, false, false, new Color(255, 255, 255, 255), 10000);
        }

        public EventHandler Clicked;

        bool press;
    }
}
