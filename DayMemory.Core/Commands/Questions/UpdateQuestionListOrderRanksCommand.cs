using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class UpdateQuestionListOrderRanksCommand : IRequest
    {
        public QuestionListItem[] QuestionLists { get; set; } = new QuestionListItem[] { };
    }

    public class QuestionListItem : IRequest
    {
        public string? QuestionListId { get; set; }

        public int OrderRank { get; set; }
    }
}
