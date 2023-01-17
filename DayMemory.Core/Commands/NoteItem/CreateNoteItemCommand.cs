using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public required string NoteId { get; set; }

        public required string NotebookId { get; set; }

        public required string Text { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }

        public long Date { get; set; }

        public bool IsEncrypted { get; set; }

        public string[] MediaFiles { get; set; } = Array.Empty<string>();

        public LocationCommandDto? Location { get; set; }

    }
}
