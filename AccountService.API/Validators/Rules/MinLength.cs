using FluentValidation;
using AccountService.API.Validators.Enums;
using AccountService.API.Validators.Extensions;
using AccountService.API.Validators.Interfaces;

namespace AccountService.API.Validators.Rules;

/// <summary>
/// Rule for minimum string length.
/// </summary>
public class MinLength : IReusableRule
{
    public string Name => "MinLength";

    private readonly int _length;

    public MinLength(int length)
    {
        _length = length;
    }

    public IRuleBuilderOptions<T, TProperty> Apply<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder)
    {
        if (typeof(TProperty) != typeof(string))
        {
            throw new InvalidOperationException("MinLength rule can only be applied to string properties.");
        }

        return (IRuleBuilderOptions<T, TProperty>)(object)
            ((IRuleBuilder<T, string>)(object)ruleBuilder)
            .MinimumLength(_length)
            .WithMessage(ValidationErrorCode.TooShort.ToErrorCode());
    }
}