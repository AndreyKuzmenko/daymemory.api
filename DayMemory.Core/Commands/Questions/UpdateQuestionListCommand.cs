using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class UpdateQuestionListCommand : IRequest
    {
        public string? QuestionListId { get; set; }

        public string? Text { get; set; }

        public string[] Questions { get; set; } = new string[] { };

        public int OrderRank { get; set; }
    }
}
