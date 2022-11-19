using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class CreateTagCommand : IRequest<string>
    {
        public string? TagId { get; set; }

        public string? Text { get; set; }

        public string? UserId { get; set; }

        public int OrderRank { get; set; }
    }
}
