﻿using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Services;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Sync.Projections;


namespace DayMemory.DAL.QueryHandlers.Notes
{
    public class GetSyncTagsQueryHandler : IRequestHandler<GetSyncTagsQuery, SyncListProjection<SyncTagProjection>>
    {
        private readonly IReadDbContext _readDbContext;

        public GetSyncTagsQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<SyncListProjection<SyncTagProjection>> Handle(GetSyncTagsQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext.GetQuery<Tag>()
                        .AsNoTracking();

            DateTimeOffset? lastSyncDateTime = request.LastSyncDateTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(request.LastSyncDateTime.Value) : null;

            var items = query
                .Where(x => x.UserId == request.UserId)
                .OrderBy(d => d.ModifiedDate)
                .Where(x => lastSyncDateTime == null || x.ModifiedDate > lastSyncDateTime)
                 .Select(entity => new SyncTagProjection()
                 {
                     Id = entity.Id,
                     Status = entity.IsDeleted ? SyncItemStatus.Deleted : SyncItemStatus.CreatedOrChanged,
                     ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     Item = entity.IsDeleted ? null : new TagProjection
                     {
                         Id = entity.Id,
                         Text = entity.Text,
                         OrderRank = entity.OrderRank,
                         IsEncrypted = entity.IsEncrypted,
                         ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     }
                 });

            var result = new SyncListProjection<SyncTagProjection>()
            {
                Count = await items.CountAsync(cancellationToken),
                Items = await items.ToListAsync(cancellationToken)
            };

            return result;
        }
    }
}