using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class UpdateNoteItemCommand : IRequest
    {
        public string? NoteId { get; set; }

        public string? NotebookId { get; set; }

        public string? Text { get; set; }

        public string[]? Images { get; set; }

        public long Date { get; set; }

        public LocationCommandDto? Location { get; set; }
    }
}
