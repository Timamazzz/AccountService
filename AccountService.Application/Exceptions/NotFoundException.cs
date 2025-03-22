namespace AccountService.Application.Exceptions;

/// <summary>
/// Exception that indicates that a requested resource was not found.
/// </summary>
public class NotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="resource">The name of the missing resource (e.g. "User").</param>
    /// <param name="key">The value used to look up the resource.</param>
    /// <param name="errorCode">Optional error code. If null, defaults to "not_found".</param>
    /// <param name="additionalData">Optional contextual data for the response.</param>
    public NotFoundException(string resource, object key, string? errorCode = null, object? additionalData = null)
        : base($"{resource} with key {key} was not found", errorCode, additionalData)
    {
    }
}