using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer.Consumers;

string rabbitMQUri = "amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj";

string queueName = "example-queue";

//Temel Konfigurasyon(pub ve consumerda var olmalı)
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);

    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();