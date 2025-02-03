using RabbitMQ.Client;
using System.Text;

//CloudAMQP üzerinden alınan URI ile bağlantı sağlama
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantıyı aktifleştirip kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Kuyruk oluşturma   //"durable: true" ile mesajın kalıcı olması sağlanıyor, böylece mesajlar kaybolmaz 
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);


//Mesajların kalıcılığı için
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

//Kuyruğa mesaj gönderme         //RabbitMQ kuyruğa atacağı mesajları byte türünde kabul ediyor, o yüzden mesajları byte'a dönüştürülmeli

for(int i =0; i<100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties); //exchange boş bırakırsak **direct exchange** kullanılır
}



Console.Read();