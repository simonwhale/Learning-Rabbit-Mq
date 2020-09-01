using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadingRabbitMqQueues.Logic
{
    public class ReadRabbitQueues : IHostedService, IDisposable
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly ILogger logger;

        public ReadRabbitQueues(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger<ReadRabbitQueues>();
            factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            channel.QueueDeclare(queue: "locationSampleQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                logger.LogInformation(message);
            };

            channel.BasicConsume(queue: "locationSampleQueue",
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

        public void Dispose()
        {
            channel?.Dispose();
            connection?.Dispose();
        }
    }
}