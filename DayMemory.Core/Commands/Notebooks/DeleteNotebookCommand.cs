using MediatR;

namespace DayMemory.Core.Commands
{
    public class DeleteNotebookCommand : IRequest
    {
        public string? NotebookId { get; set; }
    }
}