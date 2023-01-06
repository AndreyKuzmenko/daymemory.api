using Azure.Core;
using DayMemory.Core.Commands;
using DayMemory.Core.Commands.Files;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Internal;
using SkiaSharp;
using System.Drawing;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;
using Image = DayMemory.Core.Models.Personal.Image;

namespace DayMemory.Core.CommandHandlers.Files
{
    internal class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, string>
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileService _fileService;
        private readonly ISystemClock _clock;

        public CreateFileCommandHandler(IFileRepository fileRepository, IFileService fileService, ISystemClock clock)
        {
            _fileRepository = fileRepository;
            _fileService = fileService;
            _clock = clock;
        }

        public async Task<string> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var fileItem = await _fileRepository.LoadByIdAsync(request.FileId!);
            if (fileItem != null)
            {
                return request.FileId!;
            }
            using (var stream = request.FormFile!.OpenReadStream())
            {
                var fileUrl = await _fileService.UploadFileToCloudStorage(stream, request.FormFile.ContentType, $"{request.UserId}/{request.FileId}");
                stream.Position = 0;
                if (request.FileType == FileType.Image)
                {
                    using (var skImage = SKImage.FromEncodedData(stream))
                    {
                        await _fileRepository.CreateAsync(new Image
                        {
                            Id = request.FileId!,
                            FileType = FileType.Image,
                            FileName = request.FormFile!.FileName,
                            FileSize = (int)request.FormFile!.Length,
                            ImageWidth = skImage.Width,
                            ImageHeight = skImage.Height,
                            UserId = "6ed54cf5-d768-4239-ab18-b73f04bc56a2",
                            FileContentType = request.FormFile.ContentType,
                            CreatedDate = _clock.UtcNow,
                            ModifiedDate = _clock.UtcNow,
                        });
                    }
                }
                else if (request.FileType == FileType.Video)
                {
                    await _fileRepository.CreateAsync(new Models.Personal.File
                    {
                        Id = request.FileId!,
                        FileType = FileType.Video,
                        FileName = request.FormFile!.FileName,
                        FileSize = (int)request.FormFile!.Length,
                        UserId = request.UserId, //"6ed54cf5-d768-4239-ab18-b73f04bc56a2",
                        FileContentType = request.FormFile.ContentType,
                        CreatedDate = _clock.UtcNow,
                        ModifiedDate = _clock.UtcNow,
                    });
                }
                else
                {
                    throw new InvalidOperationException("Unknown file type");
                }
            }

            return request.FileId!;
        }
    }
}
