using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.Shared.Messages
{
    //Mesaj formatlarını burada belirtiyoruz
    public interface IMessage
    {
        public string Text { get; set; }
    }
}
