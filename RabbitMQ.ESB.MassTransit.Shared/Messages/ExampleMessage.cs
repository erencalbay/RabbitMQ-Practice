using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.Shared.Messages
{
    //Gerçek mesaj burada olacak
    public class ExampleMessage : IMessage
    {
        public string Text { get ; set; }
    }
}
