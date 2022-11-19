namespace DayMemory.Core.Queries.Categories.Projections
{
    public class TagProjection
    {
        public string? Id { get; set; }

        public string? Text { get; set; }

        public int OrderRank { get; set; }

        public long? ModifiedDate { get; set; }
    }
}