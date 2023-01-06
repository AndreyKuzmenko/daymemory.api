using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class DeleteNotebookCommand : IRequest
    {
        public string? NotebookId { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }
    }
}