using MassTransit;
using WebAPI.Models;
using WebAPI.Consumer.Wompi.Controllers;
using WebAPI.Consumer.Wompi;
using Models;
using Microsoft.AspNetCore.Mvc;

internal class WeatherConsumerLowTemperature : IConsumer<WeatherForecast>
{
    WeatherForecastLowTemperatureController controller = new ();
    public WeatherConsumerLowTemperature(ILogger<WeatherConsumerLowTemperature> logger)
    {
        Logger = logger;
    }

    public ILogger<WeatherConsumerLowTemperature> Logger { get; }

    public async Task Consume(ConsumeContext<WeatherForecast> context)
    {
        if (context.Message.TemperatureC > 0)
            throw new WeatherForecastException($"Higth temperature by 0 for this consumer! ({context.Message.TemperatureC} C)");
        string message = $"{context.Message.Date} {context.Message.TemperatureC} C";
        BasicResponse respond;
        try
        {
            var response = await controller.Post(context.Message);
            Logger.LogInformation(message);            
            respond = new BasicResponse()
            {
                Message = "Ok",
                Success = true,
            };
        }
        catch (Exception ex)
        {
            respond = new BasicResponse()
            {
                Message = ex.Message,
                Success = false,
            };
        }
        context.Respond(respond);
    }
}