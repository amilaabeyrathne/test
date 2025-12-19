using System.Net;
using Backend.Domain.Exceptions;

namespace Backend.WebApi.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error, message) = exception switch
        {
            TransactionLimitExceededException tle => (
                HttpStatusCode.TooManyRequests,
                "transaction_limit_exceeded",
                tle.Message),
            TransactionAlreadyExistException tae => (
                HttpStatusCode.Conflict,
                "transaction_already_exists",
                tae.Message),
            _ => (
                HttpStatusCode.InternalServerError,
                "internal_error",
                "An unexpected error occurred.")
        };

        _logger.LogError(exception, "Unhandled exception: {Error}", error);

        if (context.Response.HasStarted)
        {
            _logger.LogWarning("The response has already started, unable to write the error response.");
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            error,
            message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(payload);
    }
}

