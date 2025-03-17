using FluentValidation.Results;
using AccountService.Core.Errors.Codes;
using AccountService.Core.Errors.Base;
using AccountService.Core.Errors.Interfaces;

namespace AccountService.API.Validators;

/// <summary>
/// Maps FluentValidation errors to our custom validation error definitions.
/// </summary>
public static class FluentValidationMapper
{
    public static IValidationError MapToValidationError(ValidationFailure failure)
    {
        if (failure.CustomState is IValidationError error)
        {
            return error;
        }
        return new ValidationError("VALIDATION_ERROR", "Validation Error", failure.ErrorMessage, "Invalid input.");
    }
}