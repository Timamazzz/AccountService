using FluentValidation;
using AccountService.API.Validators.Enums;
using AccountService.API.Validators.Extensions;
using AccountService.API.Validators.Interfaces;

namespace AccountService.API.Validators.Rules;

/// <summary>
/// Rule for required (not empty) fields.
/// </summary>
public class Required : IReusableRule
{
    public string Name => "Required";

    public IRuleBuilderOptions<T, TProperty> Apply<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(ValidationErrorCode.IsRequired.ToErrorCode());
    }
}