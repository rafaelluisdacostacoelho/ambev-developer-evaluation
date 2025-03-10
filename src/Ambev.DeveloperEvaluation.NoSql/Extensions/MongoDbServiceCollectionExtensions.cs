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

        try
        {
            // Registra o MongoClient como Singleton (reutilizável em toda a aplicação)
            services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));

            // Registra o IMongoDatabase como Singleton também (não há necessidade de escopo)
            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            // Registra o contexto do MongoDB (se necessário)
            services.AddScoped<StoreNoSqlContext>();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Falha ao configurar o MongoDB.", ex);
        }

        return services;
    }
}
