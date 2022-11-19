using DayMemory.Core.Queries.Categories.Projections;

namespace DayMemory.Core.Queries.Projections
{
    public class NoteItemProjection
    {
        public string? Id { get; set; }

        public string? Text { get; set; }
            
        public long? Date { get; set; }

        public long? ModifiedDate { get; set; }

        public List<ImageProjection>? Images { get; set; }

        public virtual LocationProjection? Location { get; set; }
    }
}