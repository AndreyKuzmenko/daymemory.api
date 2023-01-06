using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class UpdateNoteItemCommand : IRequest
    {
        public required string NoteId { get; set; }

        public required string NotebookId { get; set; }

        public required string Text { get; set; }

        public string[] Files { get; set; } = new string[] { };

        public long Date { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }

        public LocationCommandDto? Location { get; set; }
    }
}
