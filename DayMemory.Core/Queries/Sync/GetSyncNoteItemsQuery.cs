using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Queries.Sync.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetSyncNoteItemsQuery : IRequest<SyncListProjection<SyncNoteItemProjection>>
    {
        public int Top { get; set; } = 10;

        public long? LastSyncDateTime { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }
    }
}