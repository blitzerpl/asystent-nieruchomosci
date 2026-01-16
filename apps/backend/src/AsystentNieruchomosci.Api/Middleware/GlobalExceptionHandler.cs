using AsystentNieruchomosci.Application.Common.Exceptions;
using AsystentNieruchomosci.Application.Common.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AsystentNieruchomosci.Api.Middleware;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    ICorrelationIdProvider correlationIdProvider,
    IWebHostEnvironment environment) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly ICorrelationIdProvider _correlationIdProvider = correlationIdProvider;
    private readonly IWebHostEnvironment _environment = environment;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var correlationId = _correlationIdProvider.GetCorrelationId() ?? "unknown";

        LogException(exception, correlationId, httpContext);

        var statusCode = MapExceptionToStatusCode(exception);
        var title = GetTitle(exception);
        var detail = GetDetail(exception);

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = GetErrorType(exception),
            Extensions =
            {
                ["correlationId"] = correlationId,
                ["timestamp"] = DateTime.UtcNow
            }
        };

        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
            if (exception.InnerException != null)
            {
                problemDetails.Extensions["innerException"] = exception.InnerException.Message;
            }
        }

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogException(Exception exception, string correlationId, HttpContext httpContext)
    {
        var logLevel = GetLogLevel(exception);
        var message = "Unhandled exception. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}";

        _logger.Log(
            logLevel,
            exception,
            message,
            correlationId,
            httpContext.Request.Path,
            httpContext.Request.Method);
    }

    private static LogLevel GetLogLevel(Exception exception)
    {
        return exception switch
        {
            ValidationException => LogLevel.Warning,
            NotFoundException => LogLevel.Information,
            ArgumentNullException => LogLevel.Warning,
            ArgumentException => LogLevel.Warning,
            _ => LogLevel.Error
        };
    }

    private static HttpStatusCode MapExceptionToStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            ArgumentNullException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };
    }

    private static string GetTitle(Exception exception)
    {
        return exception switch
        {
            ValidationException => "Validation Error",
            NotFoundException => "Resource Not Found",
            ArgumentNullException => "Missing Required Argument",
            ArgumentException => "Invalid Argument",
            UnauthorizedAccessException => "Unauthorized",
            _ => "An error occurred while processing your request"
        };
    }

    private string GetDetail(Exception exception)
    {
        if (!_environment.IsDevelopment())
        {
            return exception switch
            {
                ValidationException => exception.Message,
                NotFoundException => exception.Message,
                ArgumentNullException => exception.Message,
                ArgumentException => exception.Message,
                UnauthorizedAccessException => "You do not have permission to access this resource.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

        return exception.Message;
    }

    private static string GetErrorType(Exception exception)
    {
        return exception switch
        {
            ValidationException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            NotFoundException => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ArgumentNullException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ArgumentException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            UnauthorizedAccessException => "https://tools.ietf.org/html/rfc7235#section-3.1",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}
