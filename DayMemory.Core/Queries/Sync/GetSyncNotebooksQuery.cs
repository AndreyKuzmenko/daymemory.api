using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Queries.Sync.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetSyncNotebooksQuery : IRequest<SyncListProjection<SyncNotebookProjection>>
    {
        [JsonIgnore]
        public required string UserId { get; set; }

        public long? LastSyncDateTime { get; set; }
    }
}