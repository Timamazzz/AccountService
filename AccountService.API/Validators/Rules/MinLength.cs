using FluentValidation;
using AccountService.API.Validators.Enums;
using AccountService.API.Validators.Extensions;
using AccountService.API.Validators.Interfaces;

namespace AccountService.API.Validators.Rules;

/// <summary>
/// Rule for minimum string length.
/// </summary>
public class MinLength(int length) : IReusableRule
{
    public string Name => "MinLength";

    public IRuleBuilderOptions<T, TProperty> Apply<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
    {
        if (typeof(TProperty) != typeof(string))
        {
            throw new InvalidOperationException("MinLength rule can only be applied to string properties.");
        }

        return (IRuleBuilderOptions<T, TProperty>)(object)
            ((IRuleBuilder<T, string>)(object)ruleBuilder)
            .MinimumLength(length)
            .WithMessage(ValidationErrorCode.TooShort.ToErrorCode());
    }
}