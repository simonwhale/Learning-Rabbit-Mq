using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WritingRabbitQueues.Interfaces;
using WritingRabbitQueues.Models;

namespace WritingRabbitQueues.Logic
{
    public class QueueManager : IQueueManager
    {
        public void AddToQueue(Location location)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "locationSampleQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var message = $"Latitude: {location.Latitude}, Longitude {location.Longitude} and Time {location.Date};";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "locationSampleQueue",
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }
}
