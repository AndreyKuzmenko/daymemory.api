using DayMemory.Core.Models.Personal;

namespace DayMemory.Core.Queries.Files.Projections
{
    public class FileProjection
    {
        public required string Id { get; set; }

        public string? Name { get; set; }

        public required string Url { get; set; }

        public int FileSize { get; set; }

        public FileType FileType { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}