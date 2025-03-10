using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Messaging.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureMessagingModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        var hostName = builder.Configuration["RabbitMQ:HostName"];
        var userName = builder.Configuration["RabbitMQ:UserName"];
        var password = builder.Configuration["RabbitMQ:Password"];
        var portString = builder.Configuration["RabbitMQ:Port"];

        if (string.IsNullOrEmpty(hostName) ||
            string.IsNullOrEmpty(userName) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(portString))
        {
            throw new InvalidOperationException("RabbitMQ configuration is missing.");
        }

        var port = int.Parse(portString);

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = port
        };

        var connection = factory.CreateConnection();

        builder.Services.AddSingleton(connection);

        builder.Services.AddSingleton<IEventPublisher, RabbitMqProducer>();
    }
}
