namespace AccountService.Application.Exceptions;

/// <summary>
/// Exception that indicates that a requested resource was not found.
/// </summary>
public class NotFoundException : AppException
{
    /// <summary>
    /// Gets the semantic status code for this exception.
    /// </summary>
    public override int StatusCode => 404;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="resource">The name of the missing resource.</param>
    /// <param name="key">The value used to identify the resource.</param>
    /// <param name="errorCode">Optional error code. Defaults to "not_found".</param>
    /// <param name="additionalData">Optional additional data.</param>
    public NotFoundException(string resource, object key, string? errorCode = null, object? additionalData = null)
        : base($"{resource} with key {key} was not found", errorCode, additionalData)
    {
    }
}