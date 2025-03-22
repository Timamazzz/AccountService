using FluentValidation;

namespace AccountService.API.Validators.Interfaces;

/// <summary>
/// Interface for reusable validation rule definitions.
/// </summary>
public interface IReusableRule
{
    string Name { get; }

    IRuleBuilderOptions<T, TProperty> Apply<T, TProperty>(IRuleBuilder<T, TProperty> ruleBuilder);
}