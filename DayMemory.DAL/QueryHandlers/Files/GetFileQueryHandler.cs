using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DayMemory.Core.Queries.Files.Projections;
using File = DayMemory.Core.Models.Personal.File;
using DayMemory.Core.Services;
using static System.Net.Mime.MediaTypeNames;

namespace DayMemory.DAL.QueryHandlers.Files
{
    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, FileProjection?>
    {
        private readonly IReadDbContext _readDbContext;

        private readonly IUrlResolver _urlResolver;

        public GetFileQueryHandler(IReadDbContext readDbContext, IUrlResolver urlResolver)
        {
            _readDbContext = readDbContext;
            _urlResolver = urlResolver;
        }

        public async Task<FileProjection?> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var query = _readDbContext!.GetQuery<File>().AsNoTracking();
            var file = await query.FirstOrDefaultAsync(x => x.Id == request.FileId! && x.UserId == request.UserId, cancellationToken);
            if (file == null)
            {
                return null;
            }

            var fileUrlTemplate = _urlResolver.GetFileUrlTemplate(request.UserId!);
            return new FileProjection()
            {
                Id = file.Id,
                FileSize = file.FileSize,
                Url = string.Format(fileUrlTemplate, file.Id),
                FileType = file.FileType,
                Name = file.FileName,
                Width = file.Width,
                Height = file.Height
            };
        }
    }
}