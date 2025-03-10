using Microsoft.AspNetCore.Builder;

namespace Ambev.DeveloperEvaluation.IoC;

public interface IModuleInitializerAsync
{
    Task InitializeAsync(WebApplicationBuilder builder);
}
