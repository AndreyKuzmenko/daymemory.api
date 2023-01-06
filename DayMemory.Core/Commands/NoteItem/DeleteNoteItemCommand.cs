using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class DeleteNoteItemCommand : IRequest
    {
        public required string NoteItemId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}