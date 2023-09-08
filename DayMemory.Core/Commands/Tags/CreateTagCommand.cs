using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class CreateTagCommand : IRequest<string>
    {
        public required string TagId { get; set; }

        public required string Text { get; set; }

        public bool IsEncrypted { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }

        public int OrderRank { get; set; }
    }
}
