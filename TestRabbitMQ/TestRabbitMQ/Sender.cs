using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRabbitMQ
{
    public class Sender : IDisposable
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        public Sender()
        {
            factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.VirtualHost = "/";
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.RequestedHeartbeat = new TimeSpan(0, 0, 1);
            factory.Ssl = new SslOption("localhost");
        }

        public void Dispose()
        {
            channel?.Dispose();
            connection?.Dispose();
        }

        public void Send(string message, string queue)
        {
            if (connection == null)
                connection = factory.CreateConnection();
            if (channel == null)
            {
                channel = connection.CreateModel();
                channel.QueueDeclare(queue, false, false, false, null);
            }
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "test-queue",
                routingKey: "warning", 
                basicProperties: null, 
                body: body);
            System.Threading.Thread.Sleep(1);
            Console.WriteLine(" sended ");
        }
    }
}
