using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRabbitMQ
{
    public class Listener
    {
        public void Check(string queue)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.VirtualHost = "/";
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.RequestedHeartbeat = new TimeSpan(0, 0, 1);
            factory.Ssl = new SslOption("localhost");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queue,
                        durable: false, // сохранение очереди, т.е. при перезапуске сервера очередь восстанавливается
                        exclusive: false, // для каждого соединения своя очередь
                        autoDelete: false, // автоматическое удаление очереди после отключения всех соединений
                        arguments: null); // время жизни сообщения, ограничение очереди
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: queue,
                                         autoAck: true, // возврат сообщения в очередь?
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Console.WriteLine(" exit ");
                }
            }

            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare(
            //            queue: queue,
            //            durable: false,
            //            exclusive: false,
            //            autoDelete: false,
            //            arguments: null);

            //        var consumer = new EventingBasicConsumer(channel);
            //        channel.BasicConsume(
            //            queue: queue,
            //            autoAck: true,
            //            consumer: consumer);

            //        Console.WriteLine(" waiting for message ");
            //        //Task.Run(() =>
            //        {
            //            //while (true)
            //            {
            //                string result = "";
            //                consumer.Received += (ch, e) =>
            //                {
            //                    //var body = ea.Body.ToArray();
            //                    //channel.BasicAck(
            //                    //    deliveryTag: ea.DeliveryTag, 
            //                    //    multiple: false);
            //                    //result = Encoding.UTF8.GetString(body);
            //                    var bodyBytes = e.Body;
            //                    var message = Encoding.UTF8.GetString(bodyBytes.ToArray());

            //                    //Processing message business
            //                    Task.Run(() =>
            //                        {
            //                            try
            //                            {
            //                                Console.WriteLine("Received message" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + ":" + message + ",ThreadId=" +
            //                                    Thread.CurrentThread.ManagedThreadId);
            //                                //Confirm that the message has been received, this behavior informs the message queue to remove the message
            //                                channel.BasicAck(e.DeliveryTag, false);
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                Console.WriteLine("Confirmation message error:" + ex.Message);
            //                            }
            //                        });

            //                    //Record the processed message and write to another queue
            //                    try
            //                    {
            //                        channel.BasicPublish(exchange: "", routingKey: "testMq_handle", basicProperties: null, body: bodyBytes);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine("Record processing message exception," + ex.Message);
            //                    };
            //                    System.Threading.Thread.Sleep(1);
            //                    //string consumerTag = channel.BasicConsume(queue, true, consumer);
            //                    //Console.WriteLine(result);
            //                };
            //            }
            //            //});
            //        }
            //    }
            //}
        }
    }
}
