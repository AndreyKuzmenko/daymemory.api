using DayMemory.Core.Queries.Notebooks.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetNotebookQuery : IRequest<NotebookProjection?>
    {
        [JsonIgnore]
        public required string NotebookId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}