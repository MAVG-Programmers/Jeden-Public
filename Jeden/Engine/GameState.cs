using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Jeden.Engine.Object;
using Jeden.Engine.Render;

namespace Jeden.Engine
{
    public class GameState
    {
        public List<GameObject> GameObjects { get; set; }
        protected IControlMap ControlMap;
        protected RenderManager RenderMgr;

        public GameState() 
        {
            GameObjects = new List<GameObject>();
            RenderMgr = new RenderManager();
        }

        //Update all GameObjects attached to this GameState
        public virtual void Update(int dTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(dTime);
            }
        }

        //Draw all GameObjects that have a RenderComponent in this GameState
        public virtual void Render(RenderWindow window)
        {
            foreach (RenderComponent rComp in RenderMgr.GetDrawables())
            {
                window.Draw(rComp);
            }
        }

        //Set the InputManager that this GameState's  ControlMap watches
        public void SetInputManager(InputManager inputmgr)
        {
            ControlMap.InputMgr = inputmgr;
        }
    }
}
