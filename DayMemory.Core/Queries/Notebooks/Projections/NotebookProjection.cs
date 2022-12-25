namespace DayMemory.Core.Queries.Notebooks.Projections
{
    public class NotebookProjection
    {
        public string? Id { get; set; }

        public string? Title { get; set; }

        public int OrderRank { get; set; }

        public bool ShowInReview { get; set; }

        public long? ModifiedDate { get; set; }
    }
}