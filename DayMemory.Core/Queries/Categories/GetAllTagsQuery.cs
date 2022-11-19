using DayMemory.Core.Queries.Categories.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetAllTagsQuery : IRequest<IList<TagProjection>>
    {
        public string? UserId { get; set; }
    }
}