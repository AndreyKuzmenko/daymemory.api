using MediatR;

namespace DayMemory.Core.Commands
{
    public class DeleteNoteItemCommand : IRequest
    {
        public string? NoteItemId { get; set; }
    }
}