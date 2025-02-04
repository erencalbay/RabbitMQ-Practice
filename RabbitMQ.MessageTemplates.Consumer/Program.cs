using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-to-Point) Tasarımı
/*string queueName = "example-p2p-queue";
channel.QueueDeclare(
   queue: queueName,
   durable: false,
   exclusive: false,
   autoDelete: false);

byte[] message = Encoding.UTF8.GetBytes("Merhaba");
channel.BasicPublish(
   exchange: string.Empty,
   routingKey: queueName,
   body: message);
*/
#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı
/*
string exchangeName = "example-pub-sub-exchange";
channel.ExchangeDeclare(
    exchange:exchangeName,
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(
       exchange: "example-pub-sub-exchange",
       routingKey: string.Empty,
       body: message);
}
*/
#endregion

#region Work Queue (İş Kuyruğu) Tasarımı
/*
string queueName = "example-work-queue";

channel.QueueDeclare(
   queue: queueName,
   durable: false,
   exclusive: false,
   autoDelete: false);


for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(
       exchange: string.Empty,
       routingKey: queueName,
       body: message);
}
*/
#endregion

#region Request/Response Tasarımı
string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string responseQueueName = channel.QueueDeclare().QueueName;

//Gelen mesajların gönderilen mesajla bağlantısına bakmak için
string correlationId = Guid.NewGuid().ToString();

//Request mesajını oluşturma ve Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = responseQueueName;

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(
       exchange: string.Empty,
       routingKey: requestQueueName,
       body: message,
       basicProperties: properties);
}

//Response kuyruğu dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: responseQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{

    //Gelen mesajla gönderilen mesaj arasındaki korelasyona bakıyoruz
    if(e.BasicProperties.CorrelationId == correlationId)
        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

#endregion

Console.Read();