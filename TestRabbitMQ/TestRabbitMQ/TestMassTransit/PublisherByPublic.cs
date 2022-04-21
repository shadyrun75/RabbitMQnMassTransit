using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;

namespace TestRabbitMQ.TestMassTransit
{
    /// <summary>
    /// Класс публикации сообщения. Publish - это отправка сообщения в очередь, которое забирает любой из Consumer, который подписан на определенные очереди.
    /// Тут используются СОБЫТИЯ, а не КОМАНДЫ.
    /// </summary>
    public class PublisherByPublic : IPublisherByPublic
    {
        readonly IPublishEndpoint _publishEndpoint;
        public PublisherByPublic(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public string City { get; set; } = "";

        public async Task Start()
        {
            while (true)
            {
                var value = ReadData();
                if (value == null)
                {
                    Console.WriteLine("Error input data. Publisher stoped.");
                    break;
                }
                await Publish(value);
            }
            await Task.CompletedTask;
        }

        private WeatherForecast ReadData()
        {
            Console.WriteLine("Enter temperature");
            var value = Console.ReadLine();
            if (Int32.TryParse(value, out var tempInt))
                return new WeatherForecast()
                {
                    City = this.City,
                    Date = DateTime.Now,
                    TemperatureC = tempInt
                };
            else
                return null;
        }

        private async Task Publish(IWeatherForecast value)
        {
            await _publishEndpoint.Publish<IWeatherForecast>(value);
        }
    }

    //public static class ServiceProviderExtensions
    //{
    //    public static void AddPublisherByPublicService(this IServiceCollection services)
    //    {
    //        services.AddSingleton<PublisherByPublic>();
    //    }
    //}
}
