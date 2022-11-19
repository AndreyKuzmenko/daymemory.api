using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Services;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.QueryHandlers
{
    public class GetAllQuestionListsQueryHandler : IRequestHandler<GetAllQuestionListQuery, IList<QuestionListProjection>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetAllQuestionListsQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<IList<QuestionListProjection>> Handle(GetAllQuestionListQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext.GetQuery<QuestionList>()
                .Include(d => d.Questions)
                .AsNoTracking();

            var items = await query
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.OrderRank).ThenByDescending(x => x.CreatedDate)
                 .Select(entity => new QuestionListProjection
                 {
                     Id = entity.Id,
                     Text = entity.Text,
                     Questions = entity.Questions.OrderBy(x => x.CreatedDate).Select(i => new QuestionProjection
                     {
                         Id = i.Id,
                         Text = i.Text
                     }).ToList(),
                 })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}