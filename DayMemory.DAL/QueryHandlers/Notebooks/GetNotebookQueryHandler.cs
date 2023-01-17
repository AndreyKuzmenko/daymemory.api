using DayMemory.Core.Queries.Notebooks.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Tags.Projections;

namespace DayMemory.DAL.QueryHandlers.Notebooks
{
    public class GetNotebookQueryHandler : IRequestHandler<GetNotebookQuery, NotebookProjection?>
    {
        private readonly IReadDbContext _readDbContext;

        public GetNotebookQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<NotebookProjection?> Handle(GetNotebookQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext!.GetQuery<Notebook>().Where(x => !x.IsDeleted)
                    .AsNoTracking();

            return await query.Select(t => new NotebookProjection()
            {
                Id = t.Id,
                Title = t.Title,
                OrderRank = t.OrderRank,
                ShowInReview = t.ShowInReview,
                SortingType = t.SortingType,
                IsEncrypted = t.IsEncrypted,
                ModifiedDate = t.ModifiedDate.ToUnixTimeMilliseconds(),
            }).FirstOrDefaultAsync(x => x.Id == request.NotebookId!, cancellationToken);
        }
    }
}