using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Messaging.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Ambev.DeveloperEvaluation.Messaging.Extensions;

public static class RabbitMqExtensions
{
    private static readonly string[] hostnames = new[] { "RabbitMQ Connection" };

    public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtenção das configurações do RabbitMQ via IConfiguration
        var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
            ?? throw new ArgumentNullException(nameof(configuration), "RabbitMQ configuration section is missing or invalid.");

        // Registro do IHostedService para gerenciar a conexão RabbitMQ
        services.AddSingleton<RabbitMqConnectionService>();
        services.AddSingleton(static sp => sp.GetRequiredService<RabbitMqConnectionService>().Connection
            ?? throw new InvalidOperationException("RabbitMQ connection is not established."));

        // Registro do produtor e do pipeline behavior
        services.AddScoped<IProducer, RabbitMqProducer>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EventPublishingBehavior<,>));

        return services;
    }
}

// Classe para representar as opções do RabbitMQ no appsettings.json
internal class RabbitMqOptions
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
}

public class RabbitMqConnectionService : IHostedService, IAsyncDisposable
{
    private readonly RabbitMqOptions _options;
    private IConnection? _connection;

    public RabbitMqConnectionService(IConfiguration configuration)
    {
        _options = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
            ?? throw new ArgumentNullException(nameof(configuration), "RabbitMQ configuration section is missing or invalid.");
    }

    public IConnection? Connection => _connection;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        try
        {
            _connection = await factory.CreateConnectionAsync([_options.HostName], "RabbitMQ Connection", cancellationToken);
            Console.WriteLine("RabbitMQ Connection established.");
        }
        catch (BrokerUnreachableException ex)
        {
            Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection != null)
        {
            await _connection.CloseAsync(cancellationToken: cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
