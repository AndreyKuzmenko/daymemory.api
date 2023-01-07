using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Services;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Files.Projections;

namespace DayMemory.DAL.QueryHandlers.Notes
{
    public class GetNoteItemQueryHandler : IRequestHandler<GetNoteItemQuery, NoteItemProjection?>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly IUrlResolver _urlResolver;

        public GetNoteItemQueryHandler(IReadDbContext readDbContext, IUrlResolver urlResolver)
        {
            _readDbContext = readDbContext;
            _urlResolver = urlResolver;
        }

        public async Task<NoteItemProjection?> Handle(GetNoteItemQuery request, CancellationToken cancellationToken)
        {
            var fileUrlTemplate = _urlResolver.GetFileUrlTemplate(request.UserId!);
            var query = _readDbContext.GetQuery<NoteItem>()
                .Include(i => i.Location)
                .Include(i => i.Files)
                .ThenInclude(x => x.File)
                .AsNoTracking();


            var item = await query
                .Where(x => x.UserId == request.UserId)
                .Where(x => !x.IsDeleted)
                 .Select(entity => new NoteItemProjection
                 {
                     Id = entity.Id,
                     NotebookId = entity.NotebookId,
                     Text = entity.Text,
                     ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     Date = entity.Date.ToUnixTimeMilliseconds(),
                     MediaFiles = entity.Files.OrderBy(x => x.OrderRank).ThenBy(x => x.File!.CreatedDate).Select(i => new FileProjection
                     {
                         Id = i.File!.Id,
                         Name = i.File.FileName,
                         Url = string.Format(fileUrlTemplate, i.File.Id),
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
                 })
                .FirstOrDefaultAsync(x => x.Id == request.NoteItemId, cancellationToken);

            return item;
        }
    }
}