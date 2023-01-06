using DayMemory.Core.Models.Personal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class CreateNotebookCommand : IRequest<string>
    {
        public required string NotebookId { get; set; }

        public required string Title { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }

        public bool ShowInReview { get; set; }

        public int OrderRank { get; set; }

        public SortingType SortingType { get; set; }
    }
}
