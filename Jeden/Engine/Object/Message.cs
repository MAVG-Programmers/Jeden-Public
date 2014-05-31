using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeden.Engine.Object
{
    /// <summary>
    /// Inter-component messages for GameObjects
    /// </summary>
    public class Message
    {
        public Message(object sender) 
        {
            Sender = sender;
        }

        public object Sender { get; set; } 
                                             
    }
}
