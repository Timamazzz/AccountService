using System.Net;
using AccountService.Core.Errors.Interfaces;

namespace AccountService.Core.Errors.Exceptions;

/// <summary>
/// Base exception for application errors (business logic and system failures).
/// </summary>
public class ApplicationException(IApplicationError error) : Exception(error.Detail)
{
    /// <summary> The associated error definition. </summary>
    public IApplicationError Error { get; } = error;

    /// <summary> HTTP status code of the error. </summary>
    public HttpStatusCode StatusCode => Error.StatusCode;
}