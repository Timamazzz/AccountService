using System.Net;

namespace AccountService.Core.Errors.Interfaces;

/// <summary>
/// Represents an application-level error (business, system).
/// </summary>
public interface IApplicationError : IErrorDefinition
{
    /// <summary> HTTP status code associated with this error. </summary>
    HttpStatusCode StatusCode { get; }
}