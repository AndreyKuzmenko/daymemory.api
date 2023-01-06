using DayMemory.Core.Models.Personal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class UpdateNotebookCommand : IRequest, IResourceRequestor
    {       
        public required string Title { get; init; }

        public int OrderRank { get; init; }

        public bool ShowInReview { get; init; }

        public SortingType SortingType { get; init; }

        [JsonIgnore]
        public string? NotebookId { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
