using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetTagQuery : IRequest<TagProjection?>
    {
        public string? TagId { get; set; }

        public string? UserId { get; set; }
    }
}