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
            var imageUrlTemplate = _urlResolver.GetImageUrlTemplate(ImageSource.Note, request.UserId!);
            var query = _readDbContext.GetQuery<NoteItem>()
                .Include(i => i.Location)
                .Include(i => i.Images)
                .ThenInclude(x => x.Image)
                .AsNoTracking();


            var topic = await query
                .Where(x => x.UserId == request.UserId)
                .Where(x => !x.IsDeleted)
                 .Select(entity => new NoteItemProjection
                 {
                     Id = entity.Id,
                     Text = entity.Text,
                     ModifiedDate = entity.ModifiedDate.ToUnixTimeMilliseconds(),
                     Date = entity.Date.ToUnixTimeMilliseconds(),
                     Images = entity.Images.OrderBy(x => x.OrderRank).ThenBy(x => x.Image!.CreatedDate).Select(i => new ImageProjection
                     {
                         Id = i.Image!.Id,
                         Name = i.Image.FileName,
                         Url = string.Format(imageUrlTemplate, i.Image.Id),
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
                .FirstOrDefaultAsync(x => x.Id == request.NoteItemId, cancellationToken);

            return topic;
        }
    }
}