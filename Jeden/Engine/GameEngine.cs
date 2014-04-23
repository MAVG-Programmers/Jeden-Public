using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;
using System.Diagnostics;

namespace Jeden.Engine
{
    public class GameEngine : IDisposable
    {
        public RenderWindow Window { get; set; }
        private GameTime DeltaTime;
        private Stack<GameState> GameStates;
        private InputManager InputMgr;

        public GameEngine()
        {
            GameStates = new Stack<GameState>();
            InputMgr = new InputManager(this);
            DeltaTime = new GameTime();
        }

        public void PushState(GameState state)
        {
            GameStates.Push(state);
        }

        public void PopState()
        {
            //Non-empty
            if (GameStates.Count > 0)
            {
                //TODO: allow state to call transition code
                GameStates.Pop();
            }
        }

        public void Run(String title) 
        {
            Window = new RenderWindow(new VideoMode(800, 600), title);
            Stopwatch stopwatch = new Stopwatch();

            Window.Closed += Window_Closed;

            stopwatch.Start();

            while (Window.IsOpen())
            {
                Window.DispatchEvents();

                DeltaTime.ElapsedGameTime = DeltaTime.TotalGameTime - stopwatch.Elapsed;
                DeltaTime.TotalGameTime = stopwatch.Elapsed;
                Update(DeltaTime.ElapsedGameTime.Milliseconds);
                Draw();
                
            }
            stopwatch.Stop();
        }

        void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }
        void Update(int dTime)
        {
            InputMgr.Update(this);
            GameStates.Peek().Update(dTime);
        }
        void Draw()
        {
            Window.Clear(Color.Magenta);
            GameStates.Peek().Render(Window);
            Window.Display();
        }

        public void Dispose()
        {

        }
    }
}
