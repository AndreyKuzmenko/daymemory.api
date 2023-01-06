using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetTagQuery : IRequest<TagProjection?>
    {
        public required string TagId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}