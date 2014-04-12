using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    class Program
    {
        static void Main(string[] args)
        {
            using (JedenGame game = new JedenGame()) 
            {
                game.Run("Jeden");
            }
        }
    }
}
