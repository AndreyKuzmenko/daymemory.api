using MediatR;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands
{
    public class LocationCommandDto
    {
        public string? Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class CreateNoteItemCommand : IRequest<string>
    {
        public string? NoteId { get; set; }

        public string? NotebookId { get; set; }

        public string? Text { get; set; }

        public string? UserId { get; set; }

        public long Date { get; set; }

        public string[]? Images { get; set; }

        public LocationCommandDto? Location { get; set; }

    }
}
