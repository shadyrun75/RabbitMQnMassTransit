using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<WeatherConsumer>();    
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("amqp://guest:guest@localhost:5672");
        config.AutoStart = true;
        config.Durable = true;                                  // сохранение очереди после перезапуска сервиса
        
        config.ReceiveEndpoint("WeatherForecast-Queue", c => 
        {
            c.ConfigureConsumer<WeatherConsumer>(context);
            
        });
    });
});

builder.Services.AddMassTransitHostedService();

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

app.Run();
