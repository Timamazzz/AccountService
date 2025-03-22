namespace AccountService.Application.Exceptions;

/// <summary>
/// Base application exception that supports error code and additional data for ProblemDetails formatting.
/// </summary>
public abstract class AppException : Exception
{
    /// <summary>
    /// Gets the machine-readable error code that can be used for front-end handling or internationalization.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets additional contextual data to be included in the ProblemDetails response.
    /// </summary>
    public object? AdditionalData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppException"/> class.
    /// </summary>
    /// <param name="message">Human-readable error message.</param>
    /// <param name="errorCode">Optional machine-readable error code. If null, it will be generated from exception type.</param>
    /// <param name="additionalData">Optional additional data to be included in the response.</param>
    protected AppException(string message, string? errorCode = null, object? additionalData = null)
        : base(message)
    {
        ErrorCode = errorCode ?? GenerateDefaultErrorCode();
        AdditionalData = additionalData;
    }

    /// <summary>
    /// Generates a fallback error code from the exception type name in snake_case format.
    /// </summary>
    /// <returns>Snake-cased error code, e.g., "not_found", "validation_failed".</returns>
    private string GenerateDefaultErrorCode()
    {
        var typeName = GetType().Name.Replace("Exception", "");
        return string.Concat(typeName.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + c.ToString().ToLower() : c.ToString().ToLower()));
    }
}