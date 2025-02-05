using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

string rabbitMQUri = "amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj";

string queueName = "example-queue";

//Temel Konfigurasyon(pub ve consumerda var olmalı)
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));

Console.Write("Gönderilecek Mesaj: ");
string message = Console.ReadLine();
await sendEndpoint.Send<IMessage>(new ExampleMessage() { Text = message});

Console.Read();