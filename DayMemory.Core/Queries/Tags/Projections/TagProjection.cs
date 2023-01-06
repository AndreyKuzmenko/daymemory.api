namespace DayMemory.Core.Queries.Tags.Projections
{
    public class TagProjection
    {
        public required string Id { get; set; }

        public required string Text { get; set; }

        public int OrderRank { get; set; }

        public required long ModifiedDate { get; set; }
    }
}