using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Publisherda olduğu gibi Exchange'i fanout exchange olarak tanımlıyoruz
channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout);

//Fanout exchangede her kuyruğa mesaj gittiğini göstermek adına kullanıcıdan herhangi bir kuyruk ismi alıyoruz ve declare ediyoruz
Console.WriteLine("Kuyruk Adını Giriniz: ");
string queueName = Console.ReadLine();
channel.QueueDeclare(
    queue:queueName,
    exclusive:false
    );

//Kuyruğa bind ediyoruz (Exchange ile kuyruk arasında ilişki kurmak diyebiliriz)
channel.QueueBind(
    queue: queueName,
    exchange: "fanout-exchange-example",
    routingKey: string.Empty);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer = consumer
    );

consumer.Received += (sender, e) =>
{
    //Mesajı alıyoruz
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();