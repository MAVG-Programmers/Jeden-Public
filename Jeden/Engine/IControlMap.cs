using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine
{
    //Interface for custom behavior on input. 
    public interface IControlMap
    {
        InputManager InputMgr { get; set; }
    }
}
