using RabbitMQ.Client;
using System.Text;

//CloudAMQP üzerinden alınan URI ile bağlantı sağlama
ConnectionFactory factory = new();
factory.Uri = new("amqps://bpgwdknj:V_pdZZSLy3C60dgHPB87KSRAwqgaKgiz@gull.rmq.cloudamqp.com/bpgwdknj");

//Bağlantıyı aktifleştirip kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Kuyruk oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//Kuyruğa mesaj gönderme         //RabbitMQ kuyruğa atacağı mesajları byte türünde kabul ediyor, o yüzden mesajları byte'a dönüştürülmeli
byte[] message = Encoding.UTF8.GetBytes("Merhaba");
channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message); //exchange boş bırakırsak **direct exchange** kullanılır

Console.Read();