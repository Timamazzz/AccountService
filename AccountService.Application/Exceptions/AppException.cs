namespace AccountService.Application.Exceptions;

/// <summary>
/// Base application exception that supports error code, additional data and semantic HTTP-like status code.
/// </summary>
public abstract class AppException : Exception
{
    /// <summary>
    /// Gets the machine-readable error code used for frontend or API logic.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets optional contextual data to include in the error response.
    /// </summary>
    public object? AdditionalData { get; }

    /// <summary>
    /// Gets the semantic status code (HTTP-like) to be returned from the API.
    /// </summary>
    public virtual int StatusCode => 500;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppException"/> class.
    /// </summary>
    /// <param name="message">Human-readable error message.</param>
    /// <param name="errorCode">Optional machine-readable error code. If null, it will be generated from the exception type.</param>
    /// <param name="additionalData">Optional additional data for ProblemDetails.</param>
    protected AppException(string message, string? errorCode = null, object? additionalData = null)
        : base(message)
    {
        ErrorCode = errorCode ?? GenerateDefaultErrorCode();
        AdditionalData = additionalData;
    }

    /// <summary>
    /// Generates a default error code from the exception type name in snake_case.
    /// </summary>
    private string GenerateDefaultErrorCode()
    {
        var typeName = GetType().Name.Replace("Exception", "");
        return string.Concat(typeName.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + c.ToString().ToLower() : c.ToString().ToLower()));
    }
}