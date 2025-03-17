using FluentValidation;
using AccountService.API.Contracts.Requests;
using AccountService.Core.Errors.Codes;

namespace AccountService.API.Validators;

/// <summary>
/// Validator for CreateUserRequest.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
                .WithMessage(ValidationErrors.Required.Detail)
                .WithState(_ => ValidationErrors.Required)
            .MinimumLength(3)
                .WithMessage(ValidationErrors.TooShort.Detail)
                .WithState(_ => ValidationErrors.TooShort);
    }
}