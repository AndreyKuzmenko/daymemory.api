using DayMemory.Core.Queries.Categories.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.QueryHandlers.Categories
{
    public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagProjection?>
    {
        private readonly IReadDbContext _readDbContext;

        public GetTagQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<TagProjection?> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext!.GetQuery<Tag>().Where(x => !x.IsDeleted)
                    .AsNoTracking();

            return await query.Select(t => new TagProjection()
            {
                Id = t.Id,
                Text = t.Text,
                OrderRank = t.OrderRank,
                ModifiedDate = t.ModifiedDate.ToUnixTimeMilliseconds(),
            }).FirstOrDefaultAsync(x => x.Id == request.TagId!, cancellationToken);
        }
    }
}