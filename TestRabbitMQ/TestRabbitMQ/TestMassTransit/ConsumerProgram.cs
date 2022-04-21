using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WebAPI.Models;

namespace TestRabbitMQ.TestMassTransit
{
    public class ConsumerProgram
    {
        public static async Task Main()
        {
            Console.Clear();
            Console.WriteLine("Enter city name for subscribe: ");
            var city = Console.ReadLine() ?? String.Empty;
            Console.WriteLine("Enter temperature for subscribe: ");
            var temperature = Console.ReadLine() ?? String.Empty;
            //var city = "Chita";
            //var temperature = "low";
            var rk = GetRoutingKey(city, temperature);

            //var services = new ServiceCollection()
            //    .AddMassTransit(x =>
            //    {
            //        x.UsingRabbitMq((context, cfg) =>
            //        {
            //            cfg.Host("localhost", "/", h =>
            //            {
            //                h.Username("guest");
            //                h.Password("guest");
            //            });

            //            cfg.ReceiveEndpoint("weather-exchange", e =>
            //            {
            //                e.Consumer<Consumer>();
            //                e.ConfigureConsumer<Consumer>(context);
            //                e.Bind<IWeatherForecast>(x =>
            //                {
            //                    x.RoutingKey = rk;
            //                    x.ExchangeType = ExchangeType.Topic;
            //                });
            //            });

            //            cfg.AutoDelete = false;
            //            cfg.AutoStart = true;
            //            cfg.Durable = true;
            //        });
            //    })
            //    .BuildServiceProvider();
            //while (Console.ReadKey().Key != ConsoleKey.Escape)
            //{
            //    Console.WriteLine(" > Press Escape for exit");
            //}

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                var queueName = $"testqueue_{city}_{temperature}";
                cfg.ReceiveEndpoint(queueName, e =>
                {
                    //e.ExchangeType = "topic";
                    //e.DeadLetterExchange = "weather-exchange";
                    //e.AutoDelete = true;
                    //e.AutoStart = true;
                    e.BindQueue = true;

                    //If you are binding the messages types to the receive endpoint that are the same as message types in the consumer,
                    //you need to disable the automatic exchange binding.
                    e.ConfigureConsumeTopology = false;
                    e.Consumer<Consumer>();
                    //e.BindDeadLetterQueue("weather-exchange", queueName, x =>
                    e.Bind("weather-exchange", x =>
                    //e.Bind<IWeatherForecast>(x =>
                    {
                        x.RoutingKey = rk;
                        x.ExchangeType = ExchangeType.Topic;
                        x.Durable = true;
                        x.AutoDelete = false;
                    });
                });

                cfg.AutoDelete = true;
                cfg.AutoStart = true;
                cfg.Durable = true;
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        static string GetRoutingKey(string city, string temperature)
        {
            if ((city == String.Empty) && (temperature == String.Empty))
                return "#";
            else
            {
                var cityRK = (city == String.Empty ? "*" : city);
                var temperatureRK = (temperature == String.Empty ? "*" : temperature);
                return $"{temperatureRK}.{cityRK}";
            };
        }
    }
}
