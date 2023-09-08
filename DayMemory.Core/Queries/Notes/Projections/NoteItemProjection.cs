using DayMemory.Core.Queries.Files.Projections;
using DayMemory.Core.Queries.Tags.Projections;

namespace DayMemory.Core.Queries.Projections
{
    public class NoteItemProjection
    {
        public required string Id { get; set; }

        public string? NotebookId { get; set; }

        public string? Text { get; set; }
            
        public required long Date { get; set; }

        public bool IsEncrypted { get; set; }
        
        public required long ModifiedDate { get; set; }

        public List<FileProjection>? MediaFiles { get; set; }

        public List<string>? Tags { get; set; }

        public virtual LocationProjection? Location { get; set; }
    }
}