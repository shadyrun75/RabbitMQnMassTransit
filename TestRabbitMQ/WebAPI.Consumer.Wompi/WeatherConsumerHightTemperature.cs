using MassTransit;
using WebAPI.Models;
using WebAPI.Consumer.Wompi.Controllers;
using WebAPI.Consumer.Wompi;
using Models;

internal class WeatherConsumerHightTemperature : IConsumer<WeatherForecast>
{
    WeatherForecastHightTemperatureController controller = new ();
    public WeatherConsumerHightTemperature(ILogger<WeatherConsumerHightTemperature> logger)
    {
        Logger = logger;
    }

    public ILogger<WeatherConsumerHightTemperature> Logger { get; }

    public async Task Consume(ConsumeContext<WeatherForecast> context)
    {
        if (context.Message.TemperatureC < 30)
            throw new WeatherForecastException($"Error! Low temperature by 30 for this consumer! ({context.Message.TemperatureC} C)");
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