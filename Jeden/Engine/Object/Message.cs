using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine.Object
{
    public class Message
    {
        public Message(object sender) 
        {
            Sender = sender;
        }

        public object Sender { get; set; } 
                                             
    }
}
