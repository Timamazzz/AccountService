using FluentValidation;
using AccountService.API.Validators.Rules;

namespace AccountService.API.Validators.Extensions;

/// <summary>
/// Fluent extensions for commonly used validation rules.
/// </summary>
public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return new Required().Apply(ruleBuilder);
    }

    public static IRuleBuilderOptions<T, string> MinLength<T>(
        this IRuleBuilder<T, string> ruleBuilder, int length)
    {
        return new MinLength(length).Apply(ruleBuilder);
    }
}