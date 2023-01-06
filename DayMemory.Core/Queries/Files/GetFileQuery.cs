using DayMemory.Core.Queries.Files.Projections;
using MediatR;
using System.Text.Json.Serialization;

namespace DayMemory.Core.Queries
{
    public class GetFileQuery : IRequest<FileProjection?>
    {
        public required string FileId { get; set; }

        [JsonIgnore]
        public required string UserId { get; set; }
    }
}