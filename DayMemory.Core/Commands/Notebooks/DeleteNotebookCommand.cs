using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Commands
{
    public class DeleteNotebookCommand : IRequest
    {
        public required string NotebookId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}