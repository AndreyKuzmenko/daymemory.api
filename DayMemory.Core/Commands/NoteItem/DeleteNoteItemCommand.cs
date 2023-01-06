using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class DeleteNoteItemCommand : IRequest
    {
        public string? NoteItemId { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }
    }
}