using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.IoC;

public static class DependencyResolver
{
    public static async Task RegisterDependencies(this WebApplicationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Inicializadores síncronos
        ExecuteInitializers<IModuleInitializer>(assembly, initializer => initializer.Initialize(builder));

        // Inicializadores assíncronos
        await ExecuteAsyncInitializers<IModuleInitializerAsync>(assembly, async initializer => await initializer.InitializeAsync(builder));
    }

    private static void ExecuteInitializers<T>(Assembly assembly, Action<T> action) where T : class
    {
        var initializers = assembly.GetTypes()
            .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .OfType<T>();

        foreach (var initializer in initializers)
        {
            action(initializer);
        }
    }

    private static async Task ExecuteAsyncInitializers<T>(Assembly assembly, Func<T, Task> action) where T : class
    {
        var asyncInitializers = assembly.GetTypes()
            .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .OfType<T>();

        foreach (var asyncInitializer in asyncInitializers)
        {
            await action(asyncInitializer);
        }
    }
}
