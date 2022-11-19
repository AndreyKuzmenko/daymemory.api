using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Queries.Sync.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetSyncNoteItemsQuery : IRequest<IList<SyncNoteItemProjection>>
    {
        public int Top { get; set; } = 10;

        public long? LastSyncDateTime { get; set; }

        public string? UserId { get; set; }
    }
}