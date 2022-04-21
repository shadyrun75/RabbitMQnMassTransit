using TestRabbitMQ.CleanRabbit;
using TestRabbitMQ.TestMassTransit;

Console.WriteLine("What you wanna test? 1 - Clean RabbitMQ; 2 - MainTransit publisher, 3 - MassTransit consumer");
switch (Console.ReadKey().Key)
{
    case ConsoleKey.D1: new RabbitController(); break;
    case ConsoleKey.D2: await PublisherProgram.Main();  break;
    case ConsoleKey.D3: await ConsumerProgram.Main();  break;
    default: Console.WriteLine("Unkown command"); break;
}