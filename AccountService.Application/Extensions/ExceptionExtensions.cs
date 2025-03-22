using System;

namespace AccountService.Application.Extensions;

/// <summary>
/// Extension methods for working with exception metadata, such as error codes.
/// </summary>
public static class ExceptionExtensions
{
    private const string ErrorCodeKey = "__errorCode";

    /// <summary>
    /// Adds a unique error code to the exception if not already present.
    /// </summary>
    /// <param name="exception">The exception to extend.</param>
    public static void AddErrorCode(this Exception exception)
    {
        if (!exception.Data.Contains(ErrorCodeKey))
        {
            // Генерируем fallback-код: snake_case по типу
            var typeName = exception.GetType().Name.Replace("Exception", "");
            var code = string.Concat(typeName.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "_" + c.ToString().ToLower() : c.ToString().ToLower()));
            exception.Data[ErrorCodeKey] = code;
        }
    }

    /// <summary>
    /// Gets the error code from the exception, or generates one if not present.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>The error code string.</returns>
    public static string GetErrorCode(this Exception exception)
    {
        if (!exception.Data.Contains(ErrorCodeKey))
        {
            exception.AddErrorCode();
        }

        return exception.Data[ErrorCodeKey]?.ToString() ?? "unknown_error";
    }
}