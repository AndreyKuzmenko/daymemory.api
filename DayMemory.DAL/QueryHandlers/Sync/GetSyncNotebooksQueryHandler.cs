﻿using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Sync.Projections;
using DayMemory.Core.Queries.Notebooks.Projections;

namespace DayMemory.DAL.QueryHandlers.Notes
{
    public class GetSyncNotebooksQueryHandler : IRequestHandler<GetSyncNotebooksQuery, IList<SyncNotebookProjection>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetSyncNotebooksQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<IList<SyncNotebookProjection>> Handle(GetSyncNotebooksQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext.GetQuery<Notebook>()
                        .AsNoTracking();

            DateTimeOffset? lastSyncDateTime = request.LastSyncDateTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(request.LastSyncDateTime.Value) : null;

            var items = await query
                .Where(x => x.UserId == request.UserId)
                .OrderBy(d => d.ModifiedDate)
                .Where(x => lastSyncDateTime == null || x.ModifiedDate > lastSyncDateTime)
                 .Select(entity => new SyncNotebookProjection()
                 {
                     Id = entity.Id,
                     Status = entity.IsDeleted ? SyncItemStatus.Deleted : SyncItemStatus.CreatedOrChanged,
                     ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     Item = entity.IsDeleted ? null : new NotebookProjection
                     {
                         Id = entity.Id,
                         Title = entity.Title,
                         ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     }
                 })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}