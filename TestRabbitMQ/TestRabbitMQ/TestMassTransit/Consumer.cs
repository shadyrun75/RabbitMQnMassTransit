using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;

namespace TestRabbitMQ.TestMassTransit
{
    public class Consumer : IConsumer<IWeatherForecast>
    {
        public async Task Consume(ConsumeContext<IWeatherForecast> context)
        {
            Console.WriteLine($" > New data: {context.Message.City} {context.Message.TemperatureC}");
        }
    }
}
