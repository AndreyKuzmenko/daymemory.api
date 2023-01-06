using DayMemory.Core.Queries.Files.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetFileQuery : IRequest<FileProjection?>
    {
        public string? FileId { get; set; }

        public string? UserId { get; set; }
    }
}