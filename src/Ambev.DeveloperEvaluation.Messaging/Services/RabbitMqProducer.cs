using Ambev.DeveloperEvaluation.Common.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Messaging.Services;

public class RabbitMqProducer : IProducer
{
    private readonly IConnection _connection;

    public RabbitMqProducer(IConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public Task PublishAsync<T>(string queueName, T message)
    {
        if (string.IsNullOrEmpty(queueName))
            throw new ArgumentNullException(nameof(queueName));

        if (message == null)
            throw new ArgumentNullException(nameof(message));

        try
        {
            using var channel = _connection.CreateModel();

            // Declaração da fila de forma síncrona
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            // Publicação da mensagem de forma síncrona
            channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: false, basicProperties: properties, body: body);

            Console.WriteLine($"Message published to queue '{queueName}': {message}");
        }
        catch (BrokerUnreachableException ex)
        {
            Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
        }
        catch (AlreadyClosedException ex)
        {
            Console.WriteLine($"RabbitMQ connection was already closed: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while publishing to RabbitMQ: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}