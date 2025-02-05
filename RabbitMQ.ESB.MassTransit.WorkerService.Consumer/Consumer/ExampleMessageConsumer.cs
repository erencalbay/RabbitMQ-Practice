using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumer
{
    internal class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Gelen mesaj: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
