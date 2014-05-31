using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Audio;
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
        /// The RenderManager of this GameState
        /// </summary>
        public RenderManager RenderMgr;
        public InputManager InputMgr;

        /// <summary>
        /// The Music of this GameState
        /// </summary>
        public Music Music;

        /// <summary>
        /// A new instance of GameState.
        /// </summary>
        public GameState() 
        {
            GameObjects = new List<GameObject>();
            RenderMgr = new RenderManager();
        }

        /// <summary>
        /// Update all GameObjects attached to this GameState
        /// </summary>
        /// <param name="gameTime">The time difference to the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            for(int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all GameObjects that have a RenderComponent in this GameState
        /// </summary>
        /// <param name="window">The RenderWindow of the GameEngine</param>
        public virtual void Render(RenderWindow window)
        {
            RenderMgr.Draw();
        }

        void PlayMusic(String filename)
        {
            Music = new Music(filename);
            Music.Play();
        }

        /// <summary>
        /// Sets the InputManager that this GameState's ControlMap watches
        /// </summary>
        /// <param name="inputmgr">The watched InputManager</param>
        public void SetInputManager(InputManager inputmgr)
        {
            InputMgr = inputmgr;
        }

        public void SetRenderTarget(RenderTarget target)
        {
            RenderMgr.Target = target;
        }

        public void SetMusic(Music music)
        {
            Music = music;
        }
    }
}
