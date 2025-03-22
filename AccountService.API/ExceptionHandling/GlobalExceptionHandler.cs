using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using AccountService.Application.Exceptions;
using AccountService.Application.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AccountService.API.ExceptionHandling;

/// <summary>
/// Centralized exception handler that converts exceptions into standardized <see cref="ProblemDetails"/> responses.
/// </summary>
public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string UnhandledExceptionMessage = "An unexpected error occurred while processing the request.";

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    /// <summary>
    /// Attempts to handle the given exception and write a ProblemDetails response.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="exception">The exception that was thrown.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the exception was handled; otherwise, <c>false</c>.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception,
        CancellationToken cancellationToken)
    {
        exception.AddErrorCode();

        logger.LogError(exception, exception is AppException ? exception.Message : UnhandledExceptionMessage);

        var problemDetails = CreateProblemDetails(context, exception);
        var json = SerializeProblemDetails(problemDetails);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsync(json, cancellationToken);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> object from the given exception and context.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="exception">The exception to convert.</param>
    /// <returns>A fully populated <see cref="ProblemDetails"/> object.</returns>
    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        var requestId = context.TraceIdentifier;
        var errorCode = exception.GetErrorCode();

        var statusCode = exception switch
        {
            AppException => GetStatusCodeFromAppException(exception),
            _ => (int)HttpStatusCode.InternalServerError
        };

        var title = ReasonPhrases.GetReasonPhrase(statusCode);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = env.IsDevelopment() ? exception.Message : null,
            Extensions =
            {
                ["traceId"] = traceId,
                ["requestId"] = requestId,
                ["errorCode"] = errorCode
            }
        };

        if (exception is AppException { AdditionalData: not null } appEx)
        {
            problem.Extensions["data"] = appEx.AdditionalData;
        }

        if (!env.IsDevelopment() || exception is AppException) return problem;
        problem.Detail = exception.ToString();
        problem.Extensions["data"] = new
        {
            exceptionType = exception.GetType().FullName,
            stackTrace = exception.StackTrace?.Split('\n').Take(5)
        };

        return problem;
    }

    /// <summary>
    /// Maps known <see cref="AppException"/> types to HTTP status codes.
    /// </summary>
    /// <param name="exception">The exception to evaluate.</param>
    /// <returns>The corresponding HTTP status code.</returns>
    private static int GetStatusCodeFromAppException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            ConflictException => (int)HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }

    /// <summary>
    /// Serializes the given <see cref="ProblemDetails"/> object to JSON.
    /// </summary>
    /// <param name="problemDetails">The <see cref="ProblemDetails"/> to serialize.</param>
    /// <returns>A JSON string representing the problem details.</returns>
    private string SerializeProblemDetails(ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails, SerializerOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to serialize ProblemDetails");
            return "{}";
        }
    }
}