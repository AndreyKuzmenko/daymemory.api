using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;

namespace DayMemory.Core.Queries.Sync.Projections
{
    public class SyncTagProjection
    {
        public string? Id { get; set; }

        public long ModifiedDate { get; set; }

        public SyncItemStatus Status { get; set; }

        public TagProjection? Item { get; set; }
    }
}