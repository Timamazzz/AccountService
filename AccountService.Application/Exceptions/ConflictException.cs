namespace AccountService.Application.Exceptions;

/// <summary>
/// Exception that indicates a conflict in application state, such as a duplicate resource.
/// </summary>
public class ConflictException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="message">Description of the conflict.</param>
    /// <param name="errorCode">Optional error code. If null, defaults to "conflict".</param>
    /// <param name="additionalData">Optional contextual data for the response.</param>
    public ConflictException(string message, string? errorCode = null, object? additionalData = null)
        : base(message, errorCode, additionalData)
    {
    }
}