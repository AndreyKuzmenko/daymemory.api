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
    public class GetQuestionListQueryHandler : IRequestHandler<GetQuestionListQuery, QuestionListProjection?>
    {
        private readonly IReadDbContext _readDbContext;

        public GetQuestionListQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<QuestionListProjection?> Handle(GetQuestionListQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext.GetQuery<QuestionList>()
                .Include(d => d.Questions)
                .AsNoTracking();

            var item = await query
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.OrderRank)
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
                .FirstOrDefaultAsync(x => x.Id == request.QuestionListId, cancellationToken);

            return item;
        }
    }
}