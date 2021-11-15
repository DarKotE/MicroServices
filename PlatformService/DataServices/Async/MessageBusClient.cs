using System.Text;
using System.Text.Json;
using PlatformService.Dto;
using RabbitMQ.Client;

namespace PlatformService.DataServices.Async;

public sealed class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]),
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
            _connection.ConnectionShutdown += (object sender, ShutdownEventArgs args) =>
                Console.WriteLine("rbmq connection is shutting down");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);
        if (_connection.IsOpen)
        {
            SendMessage(_channel, message);
        }

        static void SendMessage(IModel channel, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("trigger", "", null, body);
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}