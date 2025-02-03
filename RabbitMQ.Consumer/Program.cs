using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Kuyruk Oluşturma                      //Kuyruk yapılandırması Publishler ile aynı olmalıdır.
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

//Kuyrukdan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", false, consumer);

//Mesajların consumerlara daha sıralı ve düzenli şekilde getirilmesini sağlar
channel.BasicQos(0, 1, false);

consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir
    // e.Body = Kuyruktaki mesajın veriyi bütünsel olarak getirecektir.
    // e.Body.Span veya e.Body.ToArray() = Kuyruktaki mesajın byte verisini getirecektir
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    // multiple'ı false yaparak sadece bu mesaj için bildirimde bulunulacağını belirtiyoruz
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();