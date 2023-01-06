using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;

namespace DayMemory.Core.Queries.Sync.Projections
{
    public class SyncNoteItemProjection
    {
        public required string Id { get; set; }

        public long ModifiedDate { get; set; }

        public SyncItemStatus Status { get; set; }

        public NoteItemProjection? Item { get; set; }
    }
}