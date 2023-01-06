using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.QueryHandlers.Tags
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, IList<TagProjection>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetAllTagsQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<IList<TagProjection>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            return await _readDbContext.GetQuery<Tag>()
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId && !x.IsDeleted)
                    .OrderBy(t => t.OrderRank)
                    .Select(t => new TagProjection() { Id = t.Id, Text = t.Text, OrderRank = t.OrderRank, ModifiedDate = t.ModifiedDate.ToUnixTimeMilliseconds() })
                    .ToListAsync(cancellationToken);
        }
    }
}