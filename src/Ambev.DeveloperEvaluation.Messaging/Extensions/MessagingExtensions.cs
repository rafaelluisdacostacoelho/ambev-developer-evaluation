using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Messaging.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Ambev.DeveloperEvaluation.Messaging.Extensions;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtenção das configurações do RabbitMQ via IConfiguration
        var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
            ?? throw new ArgumentNullException(nameof(configuration), "RabbitMQ configuration section is missing or invalid.");

        // Registro do IHostedService para gerenciar a conexão RabbitMQ
        services.AddSingleton<RabbitMqConnectionService>();
        services.AddSingleton<IConnection>(sp =>
            sp.GetRequiredService<RabbitMqConnectionService>().Connection
            ?? throw new InvalidOperationException("RabbitMQ connection is not established."));

        // Registro do produtor e do pipeline behavior
        services.AddScoped<IEventPublisher, RabbitMqProducer>();

        return services;
    }
}

// Classe para representar as opções do RabbitMQ no appsettings.json
internal class RabbitMqOptions
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; } = 5672; // Porta padrão do RabbitMQ
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
}

public class RabbitMqConnectionService : IHostedService, IDisposable
{
    private readonly RabbitMqOptions _options;
    private IConnection? _connection;

    public RabbitMqConnectionService(IConfiguration configuration)
    {
        _options = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
            ?? throw new ArgumentNullException(nameof(configuration), "RabbitMQ configuration section is missing or invalid.");
    }

    public IConnection? Connection => _connection;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            AutomaticRecoveryEnabled = true, // Habilitar a recuperação automática
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10) // Intervalo de recuperação
        };

        try
        {
            _connection = factory.CreateConnection(new[] { _options.HostName }, "RabbitMQ Connection");
            Console.WriteLine("RabbitMQ Connection established.");
        }
        catch (BrokerUnreachableException ex)
        {
            Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            Console.WriteLine("RabbitMQ Connection closed.");
        }
        GC.SuppressFinalize(this);
    }
}