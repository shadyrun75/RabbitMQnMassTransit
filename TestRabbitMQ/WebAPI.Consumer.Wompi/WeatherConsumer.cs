using MassTransit;
using WebAPI.Models;
using WebAPI.Consumer.Wompi.Controllers;
using WebAPI.Consumer.Wompi;
using Models;

internal class WeatherConsumer : IConsumer<WeatherForecast>
{
    public WeatherConsumer(ILogger<WeatherConsumer> logger)
    {
        Logger = logger;
    }

    public ILogger<WeatherConsumer> Logger { get; }

    public async Task Consume(ConsumeContext<WeatherForecast> context)
    {
        if (context.Message.TemperatureC > 60)
            throw new WeatherForecastException($"Very higth temperature! ({context.Message.TemperatureC} C)");
        string message = $"{context.Message.Date} {context.Message.TemperatureC} C";
        WeatherForecastController controller = new WeatherForecastController();
        await controller.Post(context.Message);
        Logger.LogInformation(message);
        context.Respond(new BasicResponse()
        { 
            Message = "Ok",
            Success = true,
        });
    }
}