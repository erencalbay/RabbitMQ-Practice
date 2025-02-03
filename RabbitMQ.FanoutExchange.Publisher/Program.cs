using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Exchange'i fanout exchange olarak tanımlıyoruz
channel.ExchangeDeclare(
    exchange: "fanout-exchange-example", 
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");

    //İlgili mesajları publish ediyoruz
    channel.BasicPublish(
        exchange: "fanout-exchange-example",
        routingKey: string.Empty,
        body:message);
}

Console.Read();