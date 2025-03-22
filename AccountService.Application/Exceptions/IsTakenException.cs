namespace AccountService.Application.Exceptions;

/// <summary>
/// Exception that indicates that a field or value is already taken (e.g., username, email).
/// </summary>
public class IsTakenException : AppException
{
    /// <summary>
    /// Gets the semantic status code for this exception.
    /// </summary>
    public override int StatusCode => 409;

    /// <summary>
    /// Initializes a new instance of the <see cref="IsTakenException"/> class.
    /// </summary>
    /// <param name="message">Human-readable error message.</param>
    /// <param name="errorCode">Optional machine-readable error code. Defaults to "is_taken".</param>
    /// <param name="additionalData">Optional additional data.</param>
    public IsTakenException(string message, string? errorCode = null, object? additionalData = null)
        : base(message, errorCode ?? "is_taken", additionalData)
    {
    }
}