using DayMemory.Core.Models.Personal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class UpdateNotebookCommand : IRequest, IResourceRequestor
    {
        [JsonIgnore]
        public string? NotebookId { get; set; }

        public required string Title { get; set; }

        public int OrderRank { get; set; }

        public bool ShowInReview { get; set; }

        public SortingType SortingType { get; set; }

        public required string UserId { get; set; }
    }
}
