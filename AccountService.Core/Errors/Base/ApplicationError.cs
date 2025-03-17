using System.Net;
using AccountService.Core.Errors.Interfaces;

namespace AccountService.Core.Errors.Base;

/// <summary>
/// Represents an application-level error (business, system).
/// </summary>
public class ApplicationError(string code, string title, string detail, int statusCode, string description)
    : IApplicationError
{
    public string Code { get; } = code;
    public string Title { get; } = title;
    public string Detail { get; } = detail;
    public string Description { get; } = description;
    public HttpStatusCode StatusCode { get; } = (HttpStatusCode)statusCode;
}