using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class CreateQuestionListCommand : IRequest<string>
    {
        public string? Text { get; set; }

        public string[] Questions { get; set; } = new string[] { };

        public string? UserId { get; set; }

        public int OrderRank { get; set; }
    }
}
