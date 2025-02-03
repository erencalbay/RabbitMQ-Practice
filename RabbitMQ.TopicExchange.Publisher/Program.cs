using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Topic Exchange'i tanımlıyoruz
channel.ExchangeDeclare(
    exchange:"topic-exchange-example",
    type:ExchangeType.Topic
    );

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    Console.WriteLine("Mesajın Gönderileceği Topic Formatını Belirtiniz : ");
    string topic = Console.ReadLine();

    //Mesajın hangi topic altında olacağını kullanıcıdan aldıktan sonra routingKey ile publish ediyoruz
    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message
        );
}

Console.Read();