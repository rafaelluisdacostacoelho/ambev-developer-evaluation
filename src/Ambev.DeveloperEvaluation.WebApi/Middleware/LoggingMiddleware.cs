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
        // Adicionando propriedades contextuais ao LogContext do Serilog
        var requestId = Activity.Current?.Id ?? context.TraceIdentifier;

        using (LogContext.PushProperty("RequestId", requestId))
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
        using (LogContext.PushProperty("HttpMethod", context.Request.Method))
        {
            _logger.LogInformation("Handling request {RequestPath} with RequestId {RequestId} using Method {HttpMethod}",
                                   context.Request.Path, requestId, context.Request.Method);

            try
            {
                await _next(context);

                _logger.LogInformation("Finished handling request {RequestPath} with status code {StatusCode} and method {HttpMethod}",
                                       context.Request.Path, context.Response.StatusCode, context.Request.Method);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request {RequestPath} with method {HttpMethod}",
                                 context.Request.Path, context.Request.Method);
                throw;
            }
        }
    }
}