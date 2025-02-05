using MassTransit;
using Microsoft.Extensions.Hosting;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Publisher
{
    public class PublishMessageService : BackgroundService
    {
        readonly IPublishEndpoint _publishEndpoint;

        public PublishMessageService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while(true)
            {
                ExampleMessage message = new()
                {
                    Text = $"{++i}. mesaj"
                };
                _publishEndpoint.Publish(message);
            }
        }
    }
}
