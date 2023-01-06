using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetAllTagsQuery : IRequest<IList<TagProjection>>
    {
        public required string UserId { get; set; }
    }
}