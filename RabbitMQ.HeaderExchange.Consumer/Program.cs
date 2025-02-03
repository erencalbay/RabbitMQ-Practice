using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Publisherda tanımlanan Header Exchange'i birebir aynı şekilde tanımlıyoruz
channel.ExchangeDeclare(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers
    );

//Sub olunacak headerı istiyoruz ve kuyruğu declare ediyoruz
Console.WriteLine("Lütfen Header Değerini Giriniz: ");
string value = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName; // Belirtilmek istenmediğinde bu şekilde RabbitMQ random olarak queue ismi atacaktır

//Exchange ile Kuyruğu bind ediyoruz
channel.QueueBind(
    queue: queueName,
    exchange: "header-exchange-example",
    routingKey: string.Empty,
    new Dictionary<string, object>
    {
        ["no"] = value
    }
    );

//Tüketilecek
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
    );

//Mesajı alıyoruz
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();