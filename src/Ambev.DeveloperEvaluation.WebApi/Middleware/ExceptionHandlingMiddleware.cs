using Ambev.DeveloperEvaluation.Application.Pagination;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, message) = HttpStatusCodeMapper.Map(ex);
            await HandleExceptionAsync(context, ex, statusCode, message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var validationErrorDetail = new ValidationErrorDetail
        {
            Error = exception.GetType().Name,
            Detail = exception.Message
        };

        var response = new ErrorResponse
        {
            Success = false,
            Message = message,
            Errors = exception is ValidationException validationException
                ? validationException.Errors.Select(error => (ValidationErrorDetail)error)
                : [validationErrorDetail]
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unexpected error: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Handled exception: {Message}", exception.Message);
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }
}

public static class HttpStatusCodeMapper
{
    public static (HttpStatusCode StatusCode, string Message) Map(Exception exception) => exception switch
    {
        ValidationException => (HttpStatusCode.BadRequest, "Validation failed"),
        UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
        ArgumentException => (HttpStatusCode.BadRequest, "Invalid argument"),
        InvalidOperationException => (HttpStatusCode.Conflict, "Invalid operation"),
        KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
        _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
    };
}
