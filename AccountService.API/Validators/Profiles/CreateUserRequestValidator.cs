using FluentValidation;
using AccountService.API.Contracts.Requests;
using AccountService.API.Validators.Extensions;

namespace AccountService.API.Validators.Profiles;

/// <summary>
/// Validator for the CreateUserRequest DTO.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .Required()
            .MinLength(3);

        RuleFor(x => x.Password)
            .Required()
            .MinLength(6);
    }
}