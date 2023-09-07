using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class UpdateNoteItemCommand : IRequest
    {
        public string? NoteId { get; set; }

        public required string NotebookId { get; set; }

        public required string Text { get; set; }

        public string[] MediaFiles { get; set; } = new string[] { };

        public long Date { get; set; }

        public bool IsEncrypted { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }

        public LocationCommandDto? Location { get; set; }
    }
}
