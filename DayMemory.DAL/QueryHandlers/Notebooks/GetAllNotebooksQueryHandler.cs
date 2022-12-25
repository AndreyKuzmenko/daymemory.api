using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Notebooks.Projections;

namespace DayMemory.DAL.QueryHandlers.Tags
{
    public class GetAllNotebooksQueryHandler : IRequestHandler<GetAllNotebooksQuery, IList<NotebookProjection>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetAllNotebooksQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<IList<NotebookProjection>> Handle(GetAllNotebooksQuery request, CancellationToken cancellationToken)
        {
            return await _readDbContext.GetQuery<Notebook>()
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId && !x.IsDeleted)
                    .OrderBy(t => t.CreatedDate)
                    .Select(t => new NotebookProjection() { Id = t.Id, Title = t.Title, OrderRank = t.OrderRank, ModifiedDate = t.ModifiedDate.ToUnixTimeMilliseconds(), })
                    .ToListAsync(cancellationToken);
        }
    }
}