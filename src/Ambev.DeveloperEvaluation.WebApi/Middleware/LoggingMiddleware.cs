using Serilog.Context;
using System.Diagnostics;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Gerando um ID de rastreamento para cada requisição
        var requestId = Activity.Current?.Id ?? context.TraceIdentifier;

        // Adicionando propriedades contextuais ao LogContext do Serilog
        using (LogContext.PushProperty("RequestId", requestId))
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        {
            _logger.LogInformation("Handling request {RequestPath} with RequestId {RequestId}", context.Request.Path, requestId);

            try
            {
                await _next(context);
                _logger.LogInformation("Finished handling request {RequestPath} with status code {StatusCode}", context.Request.Path, context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request {RequestPath}", context.Request.Path);
                throw;
            }
        }
    }
}