using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class UpdateTagCommand : IRequest
    {
        public string? TagId { get; set; }

        public string? Text { get; set; }

        public int OrderRank { get; set; }
    }
}
