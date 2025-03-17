using AccountService.Core.Errors.Codes;
using AccountService.Core.Errors.Interfaces;

namespace AccountService.Core.Errors.Exceptions;

/// <summary>
/// Exception for validation errors.
/// </summary>
public class ValidationException : Exception
{
    /// <summary> The general validation error. </summary>
    public IValidationError Error => ValidationErrors.ValidationFailure;

    /// <summary> List of validation errors per field. </summary>
    public Dictionary<string, Dictionary<string, object>> Fields { get; }

    public ValidationException(Dictionary<string, IValidationError> fieldErrors)
        : base(ValidationErrors.ValidationFailure.Detail)
    {
        Fields = fieldErrors.ToDictionary(
            e => e.Key,
            e => new Dictionary<string, object>
            {
                ["code"] = e.Value.Code,
                ["detail"] = e.Value.Detail
            }
        );
    }
}