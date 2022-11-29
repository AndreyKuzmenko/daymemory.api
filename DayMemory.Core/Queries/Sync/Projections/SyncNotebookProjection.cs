using DayMemory.Core.Queries.Notebooks.Projections;

namespace DayMemory.Core.Queries.Sync.Projections
{
    public class SyncNotebookProjection
    {
        public string? Id { get; set; }

        public long ModifiedDate { get; set; }

        public SyncItemStatus Status { get; set; }

        public NotebookProjection? Item { get; set; }
    }
}