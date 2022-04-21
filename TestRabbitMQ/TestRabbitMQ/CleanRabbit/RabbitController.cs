using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRabbitMQ.CleanRabbit
{
    public class RabbitController
    {
        readonly string queue = "testQueue";

        public RabbitController()
        {
            Console.WriteLine("Choose your process: 1 - send message, 2 - check message [default send message]");
            var command = Console.ReadLine();
            switch (command)
            {
                case "2": new Listener().Check(queue); break;
                default: Console.WriteLine(" Enter the messages (stop word - exit)"); SendMessage(); break;
            }
        }
        void SendMessage()
        {
            using (var sender = new Sender())
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message.ToLower() == "exit")
                        break;
                    sender.Send(message, queue);
                }
        }
    }
}
