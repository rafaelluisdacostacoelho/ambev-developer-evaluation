using Ambev.DeveloperEvaluation.Common.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Messaging.Services;

public class RabbitMqProducer : IEventPublisher
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqProducer> _logger;

    public RabbitMqProducer(IConnection connection, ILogger<RabbitMqProducer> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishEventAsync<T>(T domainEvent) where T : class
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var queueName = typeof(T).Name;

        try
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(domainEvent);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: false, basicProperties: properties, body: body);

            _logger.LogInformation("Mensagem publicada na fila '{QueueName}': {DomainEvent}", queueName, JsonSerializer.Serialize(domainEvent));
        }
        catch (BrokerUnreachableException ex)
        {
            _logger.LogError(ex, "Falha ao conectar ao RabbitMQ.");
        }
        catch (AlreadyClosedException ex)
        {
            _logger.LogError(ex, "Conexão com o RabbitMQ já estava fechada.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao publicar no RabbitMQ.");
        }

        await Task.CompletedTask;
    }
}