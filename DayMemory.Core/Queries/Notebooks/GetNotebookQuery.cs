using DayMemory.Core.Queries.Notebooks.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetNotebookQuery : IRequest<NotebookProjection?>
    {
        public string? NotebookId { get; set; }

        public string? UserId { get; set; }
    }
}