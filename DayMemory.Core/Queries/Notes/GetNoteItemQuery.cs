using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetNoteItemQuery : IRequest<NoteItemProjection>
    {
        public required string NoteItemId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}