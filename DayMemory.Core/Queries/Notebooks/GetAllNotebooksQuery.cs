using DayMemory.Core.Queries.Notebooks.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetAllNotebooksQuery : IRequest<IList<NotebookProjection>>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}