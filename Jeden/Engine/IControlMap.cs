using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine
{
    /// <summary>
    /// Interface for custom behavior on input.
    /// </summary> 
    public interface IControlMap
    {
        /// <summary>
        /// The InputManager that is watched.
        /// </summary>
        InputManager InputMgr { get; set; }
    }
}
