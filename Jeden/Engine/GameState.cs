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
    /// <summary>
    /// Represents a state of the game.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// The list of maintained GameObjects.
        /// </summary>
        public List<GameObject> GameObjects { get; set; }

        /// <summary>
        /// The control map for this GameState.
        /// </summary>
        protected IControlMap ControlMap;

        /// <summary>
        /// The RenderManager of this GameState
        /// </summary>
        protected RenderManager RenderMgr;

        /// <summary>
        /// A new instance of GameState.
        /// </summary>
        public GameState() 
        {
            GameObjects = new List<GameObject>();
            RenderMgr = new RenderManager();
        }

        //Update all GameObjects attached to this GameState
        /// <summary>
        /// Update all GameObjects attached to this GameState
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all GameObjects that have a RenderComponent in this GameState
        /// </summary>
        /// <param name="window">The RenderWindow of the GameEngine</param>
        public virtual void Render(RenderWindow window)
        {
            foreach (RenderComponent renderComponent in RenderMgr.GetDrawables())
            {
                window.Draw(renderComponent);
            }
        }

        /// <summary>
        /// Sets the InputManager that this GameState's ControlMap watches
        /// </summary>
        /// <param name="inputmgr">The watched InputManager</param>
        public void SetInputManager(InputManager inputManager)
        {
            ControlMap.InputMgr = inputManager;
        }
    }
}
