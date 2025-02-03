//Bağlantı Oluşturma
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Exchange'i direct olarak tanımlıyoruz ve isimlendiriyoruz
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.WriteLine("Mesaj : ");
    string message = Console.ReadLine();
    byte[] bytemessage = Encoding.UTF8.GetBytes(message);

    //Gönderilecek olan mesajın hangi kuyruğa gideceğini routing key ile belirtiyoruz
    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-queue-example",
        body: bytemessage);
}

Console.Read();