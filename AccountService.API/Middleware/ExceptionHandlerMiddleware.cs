using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AccountService.Core.Errors.Exceptions;

namespace AccountService.API.Middleware;

/// <summary>
/// Middleware for handling global exceptions and converting them to ProblemDetails responses.
/// </summary>
public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private const string ErrorDomain = "https://localhost:8080/errors";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Middleware pipeline execution with centralized exception handling.
    /// </summary>
    /// <param name="context">The HTTP context of the current request.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Core.Errors.Exceptions.ApplicationException ex)
        {
            await HandleApplicationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnexpectedExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles validation exceptions and formats them into a structured response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The validation exception.</param>
    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Type = $"{ErrorDomain}/{exception.Error.Code}",
            Title = "Validation Error",
            Status = (int)HttpStatusCode.BadRequest,
            Detail = "One or more validation errors occurred.",
            Extensions =
            {
                ["code"] = exception.Error.Code,
                ["fields"] = exception.Fields
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, _jsonOptions));
    }

    /// <summary>
    /// Handles application-specific exceptions and formats them into a structured response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The application exception.</param>
    private async Task HandleApplicationExceptionAsync(HttpContext context,
        Core.Errors.Exceptions.ApplicationException exception)
    {
        context.Response.StatusCode = (int)exception.StatusCode;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Type = $"{ErrorDomain}/{exception.Error.Code}",
            Title = exception.Error.Title,
            Status = (int)exception.StatusCode,
            Detail = exception.Error.Detail,
            Extensions = { ["code"] = exception.Error.Code }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, _jsonOptions));
    }

    /// <summary>
    /// Handles unexpected exceptions and returns a generic 500 Internal Server Error response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The unexpected exception.</param>
    private async Task HandleUnexpectedExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Type = $"{ErrorDomain}/internal-server-error",
            Title = "Internal Server Error",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = "An unexpected error occurred. Please try again later.",
            Extensions = { ["code"] = "internal_server_error" }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, _jsonOptions));
    }
}