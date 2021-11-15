using System.Text;
using System.Text.Json;
using PlatformService.Dto;
using RabbitMQ.Client;

namespace PlatformService.DataServices.Async;

public sealed class MessageBusClient : IMessageBusClient
{
    private readonly IConnection? _connection;
    private readonly IModel? _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"]),
        };

        try
        {
            Console.WriteLine("RabbitMQ connection is being created.");
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
            _connection.ConnectionShutdown += (_, _) =>
                Console.WriteLine("RabbitMQ connection is shutting down");
        }
        catch (Exception e)
        {
            Console.WriteLine("RabbitMQ connection failed to be created." +  e.Message);
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);
        if (_connection is { IsOpen: true })
        {
            if (_channel != null) SendMessage(_channel, message);
        }

        static void SendMessage(IModel channel, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("trigger", "", null, body);
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}