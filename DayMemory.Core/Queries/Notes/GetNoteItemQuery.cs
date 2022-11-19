using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetNoteItemQuery : IRequest<NoteItemProjection>
    {
        public string? NoteItemId { get; set; }

        public string? UserId { get; set; }
    }
}