using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    public class PlayerCharacter : GameObject
    {
        public PlayerCharacter(JedenGame game) 
        {
            Components.Add(typeof(UserInputComponent), new UserInputComponent(this));
        }
    }
}
