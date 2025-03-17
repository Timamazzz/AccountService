using AccountService.Core.Errors.Base;
using AccountService.Core.Errors.Interfaces;

namespace AccountService.Core.Errors.Codes;

/// <summary>
/// Predefined validation errors.
/// </summary>
public static class ValidationErrors
{
    /// <summary> Generic validation failure. </summary>
    public static readonly IValidationError ValidationFailure = new ValidationError(
        "VALIDATION_ERROR",
        "Validation Error",
        "One or more validation errors occurred.",
        "The input data did not meet the required validation criteria."
    );

    /// <summary> The field is required. </summary>
    public static readonly IValidationError Required = new ValidationError(
        "REQUIRED",
        "Field Required",
        "This field is required.",
        "Ensure that the field is included in the request and is not empty."
    );

    /// <summary> The field is too short. </summary>
    public static readonly IValidationError TooShort = new ValidationError(
        "TOO_SHORT",
        "Field Too Short",
        "The value is too short.",
        "Ensure that the input meets the minimum length requirement."
    );

    /// <summary> The field is too long. </summary>
    public static readonly IValidationError TooLong = new ValidationError(
        "TOO_LONG",
        "Field Too Long",
        "The value is too long.",
        "Ensure that the input does not exceed the maximum length allowed."
    );

    /// <summary> The field contains invalid characters. </summary>
    public static readonly IValidationError InvalidCharacters = new ValidationError(
        "INVALID_CHARACTERS",
        "Invalid Characters",
        "This field contains invalid characters.",
        "Ensure that the input contains only allowed characters."
    );
}