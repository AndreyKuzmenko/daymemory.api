using FluentValidation;
using DayMemory.Core.Commands.BackOffice.Topics;
using DayMemory.Core.Commands;
using DayMemory.Core.Extensions;
using System;

namespace DayMemory.Core.Commands.BackOffice.Topics.Validators
{
    public class CreateNoteItemCommandValidator : AbstractValidator<CreateNoteItemCommand>
    {
        public CreateNoteItemCommandValidator()
        {
            When(c => !string.IsNullOrEmpty(c.NoteId), () =>
            {
                RuleFor(c => c.NoteId!).IdMustBeCorrect();
            });
        }
    }
}