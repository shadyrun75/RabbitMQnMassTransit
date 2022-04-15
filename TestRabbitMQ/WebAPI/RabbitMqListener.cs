using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Diagnostics;
using System;

namespace WebAPI
{
    public class RabbitMqListener : BackgroundService
    {
        readonly string queue = "testQueue";
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqListener()
        {
            Console.WriteLine("Create rabbit mq listener");
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.VirtualHost = "/";
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.RequestedHeartbeat = new TimeSpan(0, 0, 1);
            factory.Ssl = new SslOption("localhost");
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                        queue: queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            try
            {
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (ch, ea) =>
                {
                    
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine($"Получено сообщение: {content}");
                    
                    _channel.BasicAck(ea.DeliveryTag, false);
                };

                _channel.BasicConsume(queue, false, consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Execute Async ERROR: {ex.ToString()}");
            }

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
