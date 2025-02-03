using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Publisherda tanımlanan Topic Exchange'i birebir aynı şekilde tanımlıyoruz
channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );

//Sub olunacak topici istiyoruz ve kuyruğu declare ediyoruz
Console.WriteLine("Dinlenicek Topic Formatını Belirtiniz: ");
string topic = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName; // Belirtilmek istenmediğinde bu şekilde RabbitMQ random olarak queue ismi atacaktır

//Exchange ile Kuyruğu bind ediyoruz
channel.QueueBind(
    queue: queueName,
    exchange: "topic-exchange-example", 
    routingKey: topic);

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