using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace readRabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "locationSampleQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    Console.WriteLine(" [x] Waiting for messages. ");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($" [x] Received {message}");

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };


                    channel.BasicConsume(queue: "locationSampleQueue",
                        autoAck: true, //autoAck : true causes RabbitMq to remove the message from the queue once deleted.
                        consumer: consumer);
                }
            }
        }
    }
}
