using DayMemory.Core.Models.Personal;
using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class UpdateNotebookCommand : IRequest
    {
        public string? NotebookId { get; set; }

        public string? Title { get; set; }

        public int OrderRank { get; set; }

        public bool ShowInReview { get; set; }

        public SortingType SortingType { get; set; }
    }
}
