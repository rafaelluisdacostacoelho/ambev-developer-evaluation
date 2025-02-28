using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.NoSql.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MongoDB:ConnectionString"];
        var databaseName = configuration["MongoDB:DatabaseName"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(configuration), "A string de conexão do MongoDB não pode ser nula ou vazia.");

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentNullException(nameof(configuration), "O nome do banco de dados MongoDB não pode ser nulo ou vazio.");

        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        services.AddScoped<StoreNoSqlContext>();

        return services;
    }
}
