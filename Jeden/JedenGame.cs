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

namespace Jeden
{
    public class JedenGame : IDisposable
    {
        public RenderWindow Window { get; set; }
        public GameTime DeltaTime { get; set; }
        public GameState ActiveGameState { get; set; }
        public InputManager UserInput { get; set; }

        public void Run(String title) 
        {
            Stopwatch stopwatch = new Stopwatch();
            DeltaTime = new GameTime();
            Window = new RenderWindow(new VideoMode(800, 600), title);
            UserInput = new InputManager(this);

            //Serialization Tests
            Console.WriteLine("Vector2I: " + typeof(Vector2i).IsSerializable);

            Window.Closed += Window_Closed;

            LoadContent();

            stopwatch.Start();

            while (Window.IsOpen())
            {
                Window.DispatchEvents();

                DeltaTime.ElapsedGameTime = DeltaTime.TotalGameTime - stopwatch.Elapsed;
                DeltaTime.TotalGameTime = stopwatch.Elapsed;
                Update();
                Draw();
                
            }
            stopwatch.Stop();
            UnloadContent();
        }

        void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }

        void LoadContent() 
        {
            //set ActiveGameState to the initial GameState
            ActiveGameState = new InGameState(this);
            ActiveGameState.LoadContent(this);
        }
        void Update()
        {
            UserInput.Update(this);
            ActiveGameState.Update(this);
        }
        void Draw()
        {
            Window.Clear(Color.Magenta);
            ActiveGameState.Draw(this);
            Window.Display();
        }
        void UnloadContent()
        {
            ActiveGameState.UnloadContent(this);
        }

        public void Dispose()
        {
            
        }
    }
}
