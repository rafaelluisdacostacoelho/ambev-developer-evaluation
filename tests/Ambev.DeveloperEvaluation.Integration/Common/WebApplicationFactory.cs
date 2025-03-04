using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Ambev.DeveloperEvaluation.Integration.Common;

public class AmbevWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    //{
    //    builder.ConfigureServices(services =>
    //    {
    //        var descriptor = services.SingleOrDefault(
    //            d => d.ServiceType == typeof(DbContextOptions<StoreDbContext>));

    //        if (descriptor != null)
    //        {
    //            services.Remove(descriptor);
    //        }

    //        services.AddDbContext<StoreDbContext>(options =>
    //        {
    //            options.UseInMemoryDatabase("TestDatabase");
    //        });
    //    });
    //}

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing"); // Define o ambiente de testes
    }
}
