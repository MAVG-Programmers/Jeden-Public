using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeden.Engine
{
    /// <summary>
    /// Represents time differences in the game.
    /// </summary>
    public class GameTime
    {
        /// <summary>
        /// The amount of game time since the start of the game.
        /// </summary>
        public TimeSpan TotalGameTime { get; set; }

        /// <summary>
        /// The amount of elapsed game time since the last update.
        /// </summary>
        public TimeSpan ElapsedGameTime { get; set; }
    }
}
