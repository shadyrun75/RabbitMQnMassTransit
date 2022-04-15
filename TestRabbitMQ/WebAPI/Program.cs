using MassTransit;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("amqp://guest:guest@localhost:5672");
    });
    x.AddRequestClient<WeatherForecast>();
    }
); 

builder.Services.AddMassTransitHostedService();

//builder.Services.AddHostedService<RabbitMqListener>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseRabbitListener();

app.Run();


//public static class ApplicationBuilderExtentions
//{
//    //the simplest way to store a single long-living object, just for example.
//    private static RabbitMqListener _listener { get; set; }

//    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
//    {
//        _listener = app.ApplicationServices.GetService<RabbitMqListener>();

//        var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

//        lifetime.ApplicationStarted.Register(OnStarted);

//        //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
//        lifetime.ApplicationStopping.Register(OnStopping);

//        return app;
//    }

//    private static void OnStarted()
//    {
//        _listener.Register();
//    }

//    private static void OnStopping()
//    {
//        _listener.Deregister();
//    }
//}