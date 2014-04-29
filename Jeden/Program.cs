using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jeden.Engine;
using Jeden.Game;

namespace Jeden
{
    class Program
    {
        static void Main(string[] args)
        {
            using (GameEngine engine = new GameEngine()) 
            {
                engine.PushState(new JedenGameState());
                engine.Run();
            }
        }
    }
}
