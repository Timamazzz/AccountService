using AccountService.Core.Errors.Interfaces;

namespace AccountService.Core.Errors.Base;

/// <summary>
/// Represents a validation error for input validation.
/// </summary>
public class ValidationError(string code, string title, string detail, string description)
    : IValidationError
{
    public string Code { get; } = code;
    public string Title { get; } = title;
    public string Detail { get; } = detail;
    public string Description { get; } = description;
}