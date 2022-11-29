using DayMemory.Core.Queries.Notebooks.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetAllNotebooksQuery : IRequest<IList<NotebookProjection>>
    {
        public string? UserId { get; set; }
    }
}