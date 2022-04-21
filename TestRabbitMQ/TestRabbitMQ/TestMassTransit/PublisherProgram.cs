using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using WebAPI.Models;
using Microsoft.Extensions.Configuration;

namespace TestRabbitMQ.TestMassTransit
{
    public class PublisherProgram
    {
        public static async Task Main()
        {
            Console.Clear();
            Console.WriteLine("Enter city name: ");
            var city = Console.ReadLine() ?? "";

            var services = new ServiceCollection()
                .AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.Message<IWeatherForecast>(c => c.SetEntityName("weather-exchange"));
                            cfg.Publish<IWeatherForecast>(c => c.ExchangeType = ExchangeType.Topic); 
                            // ExchangeType:
                            // direct работает только по routingkey - одному слову
                            // fanout работает со всеми подписчиками игнорируя routingkey
                            // topic работает c подписчиками по bind и routingkey может быть составной *.word.# где * - одно слово, # - 0 или много слов
                            // headers работает только с аттрибутами и значениями, игнорируя routingkey
                            cfg.Send<IWeatherForecast>(c => c.UseRoutingKeyFormatter(cnt =>
                            {
                                var temperature = cnt.Message.TemperatureC > 0 ? "hight" : cnt.Message.TemperatureC < 0 ? "low" : "zero";
                                return $"{temperature}.{cnt.Message.City}";
                            })); 
                            cfg.AutoDelete = true;
                            cfg.AutoStart = true;
                            cfg.Durable = true;                            
                        });
                    })
                .AddSingleton<IPublisherByPublic, PublisherByPublic>()
                .BuildServiceProvider();

            var publisher = services.GetService<IPublisherByPublic>();
            publisher.City = city;
            await publisher.Start();            
        }
    }
}
