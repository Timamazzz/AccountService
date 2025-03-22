using AccountService.API.Validators.Enums;

namespace AccountService.API.Validators.Extensions;

/// <summary>
/// Extension methods for converting <see cref="ValidationErrorCode"/> to string codes.
/// </summary>
public static class ValidationErrorCodeExtensions
{
    public static string ToErrorCode(this ValidationErrorCode code)
    {
        return code switch
        {
            ValidationErrorCode.IsRequired => "is_required",
            ValidationErrorCode.TooShort => "too_short",
            _ => "validation_error"
        };
    }
}