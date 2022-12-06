using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;

namespace DayMemory.Core.Queries.Sync.Projections
{
    public class SyncListProjection<T>
    {
        public int Count { get; set; }

        public List<T>? Items { get; set; }
    }
}