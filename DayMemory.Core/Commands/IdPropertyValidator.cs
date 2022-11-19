using FluentValidation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayMemory.Core;

namespace DayMemory.Core.Commands
{
    public class IdPropertyValidator<T> : PropertyValidator<T, string>
    {
        public override string Name => "IdPropertyValidator";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            var isValid = !string.IsNullOrWhiteSpace(value) && value.Length <= Constants.IdMaxLength;
            if (isValid)
            {
                return true;
            }

            context.MessageFormatter.AppendArgument("IdMaxLength", Constants.IdMaxLength);
            context.MessageFormatter.AppendArgument("EnteredLength", value?.Length ?? 0);

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' must be less than or equal to '{IdMaxLength}' characters. You entered '{EnteredLength}' characters.";
    }
}
