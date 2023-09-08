using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Services;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Sync.Projections;
using DayMemory.Core.Queries.Files.Projections;

namespace DayMemory.DAL.QueryHandlers.Notes
{
    public class GetSyncNoteItemsQueryHandler : IRequestHandler<GetSyncNoteItemsQuery, SyncListProjection<SyncNoteItemProjection>>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly IUrlResolver _urlResolver;

        public GetSyncNoteItemsQueryHandler(IReadDbContext readDbContext, IUrlResolver urlResolver)
        {
            _readDbContext = readDbContext;
            _urlResolver = urlResolver;
        }

        public async Task<SyncListProjection<SyncNoteItemProjection>> Handle(GetSyncNoteItemsQuery request, CancellationToken cancellationToken)
        {
            var originalFileUrlTemplate = _urlResolver.GetOriginalFileUrlTemplate(request.UserId!);
            var resizedFileUrlTemplate = _urlResolver.GetResizedFileUrlTemplate(request.UserId!);

            var query = _readDbContext.GetQuery<NoteItem>()
                .Include(i => i.Location)
                .Include(i => i.Files)
                .ThenInclude(x => x.File)
                .AsNoTracking();

            DateTimeOffset? lastSyncDateTime = request.LastSyncDateTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(request.LastSyncDateTime.Value) : null;

            var items = query
                .Where(x => x.UserId == request.UserId)
                .OrderBy(d => d.ModifiedDate)
                .Where(x => lastSyncDateTime == null || x.ModifiedDate > lastSyncDateTime)

                 .Select(entity => new SyncNoteItemProjection()
                 {
                     Id = entity.Id,
                     Status = entity.IsDeleted ? SyncItemStatus.Deleted : SyncItemStatus.CreatedOrChanged,
                     ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     Item = entity.IsDeleted ? null : new NoteItemProjection
                     {
                         Id = entity.Id,
                         Text = entity.Text,
                         NotebookId = entity.NotebookId,
                         ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                         Date = entity.Date.ToUnixTimeMilliseconds(),
                         IsEncrypted = entity.IsEncrypted,
                         Tags = entity.Tags.Select(x => x.TagId!).ToList(),
                         MediaFiles = entity.Files.OrderBy(x => x.OrderRank).ThenBy(x => x.File!.CreatedDate).Select(i => new FileProjection
                         {
                             Id = i.File!.Id,
                             Name = i.File.FileName,
                             OriginalUrl = string.Format(originalFileUrlTemplate, i.File.Id),
                             ResizedUrl = string.Format(i.File.FileType == FileType.Video ? originalFileUrlTemplate : resizedFileUrlTemplate, i.File.Id),
                             FileSize = i.File.FileSize,
                             FileType = i.File.FileType,
                             Width = i.File.Width,
                             Height = i.File.Height
                         }).ToList(),
                         Location = entity.Location != null ? new LocationProjection()
                         {
                             Address = entity.Location.Address,
                             Latitude = entity.Location.Latitude,
                             Longitude = entity.Location.Longitude
                         } : null,
                     }
                 });

            var result = new SyncListProjection<SyncNoteItemProjection>()
            {
                Count = await items.CountAsync(cancellationToken),
                Items = await items.Take(request.Top).ToListAsync(cancellationToken)
            };

            return result;
        }
    }
}