using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetAllNoteItemsQuery : IRequest<IList<NoteItemProjection>>
    {
        public int Top { get; set; } = 10;

        public long? LastItemDateTime { get; set; }

        public string? Tag { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}