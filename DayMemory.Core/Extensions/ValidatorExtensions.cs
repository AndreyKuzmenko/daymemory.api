using DayMemory.Core.Commands;
using FluentValidation;

namespace DayMemory.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IdMustBeCorrect<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IdPropertyValidator<T>());
        }
    }
}