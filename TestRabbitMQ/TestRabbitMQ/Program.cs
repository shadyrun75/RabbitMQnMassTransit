using TestRabbitMQ;

string queue = "testQueue";
Console.WriteLine("Choose your process: 1 - send message, 2 - check message [default send message], 3 - WebAPI send");
var command = Console.ReadLine();
switch (command)
{
    case "2": new Listener().Check(queue); break;
    case "3": new SenderRESTApi().Send(queue); break;
    default: Console.WriteLine(" Enter the messages (stop word - exit)"); SendMessage(); break;
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