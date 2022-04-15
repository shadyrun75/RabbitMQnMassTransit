using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRabbitMQ
{
    public class SenderRESTApi : IDisposable
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        public SenderRESTApi()
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

        public void Send(string queue, byte[]? body = null)
        {
            if (connection == null)
                connection = factory.CreateConnection();
            if (channel == null)
            {
                channel = connection.CreateModel();
                
                channel.QueueDeclare(queue, false, false, false, null);
            }
            channel.BasicPublish("", queue, null, body);
            System.Threading.Thread.Sleep(1);
            Console.WriteLine(" sended ");
        }
    }
}
