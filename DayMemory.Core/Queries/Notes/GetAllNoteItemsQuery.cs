using DayMemory.Core.Queries.Categories.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetAllNoteItemsQuery : IRequest<IList<NoteItemProjection>>
    {
        public int Top { get; set; } = 10;

        public long? LastItemDateTime { get; set; }

        public string? Tag { get; set; }

        public string? UserId { get; set; }
    }
}