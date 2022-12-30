using DayMemory.Core.Models.Personal;
using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class CreateNotebookCommand : IRequest<string>
    {
        public string? NotebookId { get; set; }

        public string? Title { get; set; }

        public string? UserId { get; set; }

        public bool ShowInReview { get; set; }

        public int OrderRank { get; set; }

        public SortingType SortingType { get; set; }
    }
}
