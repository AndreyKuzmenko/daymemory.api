namespace DayMemory.Core.Queries.Files.Projections
{
    public class FileProjection
    {
        public required string Id { get; set; }

        public string? Name { get; set; }

        public required string Url { get; set; }

        public int FileSize { get; set; }
    }
}