using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using AccountService.Application.Exceptions;
using AccountService.Application.Exceptions.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AccountService.API.ExceptionHandling;

/// <summary>
/// Centralized exception handler that converts exceptions into standardized <see cref="ProblemDetails"/> responses.
/// </summary>
public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception,
        CancellationToken cancellationToken)
    {
        exception.AddErrorCode();

        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        var requestId = context.TraceIdentifier;
        var errorCode = exception.GetErrorCode();

        logger.LogError(exception,
            "Unhandled exception: {ExceptionType} | Status: {Status} | Path: {Path} | ErrorCode: {ErrorCode} | TraceId: {TraceId} | RequestId: {RequestId}",
            exception.GetType().Name,
            exception is AppException appEx ? appEx.StatusCode : 500,
            context.Request.Path,
            errorCode,
            traceId,
            requestId);

        var problemDetails = CreateProblemDetails(context, exception, traceId, requestId, errorCode);
        var json = SerializeProblemDetails(problemDetails);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsync(json, cancellationToken);
        return true;
    }

    private ProblemDetails CreateProblemDetails(
        HttpContext context,
        Exception exception,
        string traceId,
        string requestId,
        string errorCode)
    {
        var statusCode = exception is AppException appEx
            ? appEx.StatusCode
            : (int)HttpStatusCode.InternalServerError;

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

        if (exception is AppException { AdditionalData: not null } withData)
        {
            problem.Extensions["data"] = withData.AdditionalData;
        }

        if (env.IsDevelopment() && exception is not AppException)
        {
            problem.Detail = exception.ToString();
            problem.Extensions["data"] = new
            {
                exceptionType = exception.GetType().FullName,
                stackTrace = exception.StackTrace?.Split('\n').Take(5)
            };
        }

        return problem;
    }

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