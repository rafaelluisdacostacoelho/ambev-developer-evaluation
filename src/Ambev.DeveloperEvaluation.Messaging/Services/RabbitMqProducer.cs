using Ambev.DeveloperEvaluation.Common.Messaging;
using RabbitMQ.Client;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Messaging.Services;

public class RabbitMqProducer : IProducer
{
    private readonly IConnection _connection;

    public RabbitMqProducer(IConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        using var channel = await _connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, mandatory: false, basicProperties: properties, body: body);
    }
}
