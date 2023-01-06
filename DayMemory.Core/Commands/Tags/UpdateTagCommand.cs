using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class UpdateTagCommand : IRequest
    {
        public required string TagId { get; set; }

        public required string Text { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }

        public int OrderRank { get; set; }
    }
}
