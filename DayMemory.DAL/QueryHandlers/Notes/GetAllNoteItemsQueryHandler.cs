using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Queries.Projections;
using DayMemory.Core.Services;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.QueryHandlers.Notes
{
    public class GetAllNoteItemsQueryHandler : IRequestHandler<GetAllNoteItemsQuery, IList<NoteItemProjection>>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly IUrlResolver _urlResolver;

        public GetAllNoteItemsQueryHandler(IReadDbContext readDbContext, IUrlResolver urlResolver)
        {
            _readDbContext = readDbContext;
            _urlResolver = urlResolver;
        }

        public async Task<IList<NoteItemProjection>> Handle(GetAllNoteItemsQuery request, CancellationToken cancellationToken)
        {
            var imageUrlTemplate = _urlResolver.GetImageUrlTemplate(ImageSource.Note, request.UserId!);

            var query = _readDbContext.GetQuery<NoteItem>()
                .Include(i => i.Location)
                .Include(i => i.Images)
                .ThenInclude(x => x.Image)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Tag))
            {
                query = query.Where(x => x.Text != null && x.Text.Contains("#" + request.Tag));
            }

            DateTimeOffset? lastItemDateTime = request.LastItemDateTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(request.LastItemDateTime.Value) : null;

            var topics = await query
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(d => d.Date)
                .Where(x => !x.IsDeleted)
                .Where(x => lastItemDateTime == null || x.Date < lastItemDateTime)
                .Take(request.Top)
                 .Select(entity => new NoteItemProjection
                 {
                     Id = entity.Id,
                     NotebookId = entity.NotebookId,
                     Text = entity.Text,
                     ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     Date = entity.Date.ToUnixTimeMilliseconds(),
                     Images = entity.Images.OrderBy(x => x.OrderRank).ThenBy(x => x.Image!.CreatedDate).Select(i => new ImageProjection
                     {
                         Id = i.Image!.Id,
                         Name = i.Image.FileName,
                         Url = string.Format(imageUrlTemplate, i.Image.Id),
                         FileSize = i.Image.FileSize,
                         Width = i.Image.Width,
                         Height = i.Image.Height
                     }).ToList(),
                     Location = entity.Location != null ? new LocationProjection()
                     {
                         Address = entity.Location.Address,
                         Latitude = entity.Location.Latitude,
                         Longitude = entity.Location.Longitude
                     } : null,
                 })
                .ToListAsync(cancellationToken);

            return topics;
        }
    }
}