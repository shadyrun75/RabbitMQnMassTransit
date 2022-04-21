
namespace TestRabbitMQ.TestMassTransit
{
    public interface IPublisherByPublic
    {
        string City { get; set; }

        Task Start();
    }
}