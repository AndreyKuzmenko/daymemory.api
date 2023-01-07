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
using File = DayMemory.Core.Models.Personal.File;

namespace DayMemory.Core.CommandHandlers.Files
{
    internal class CreateMediaFileCommandHandler : IRequestHandler<CreateMediaFileCommand, string>
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileService _fileService;
        private readonly ISystemClock _clock;

        public CreateMediaFileCommandHandler(IFileRepository fileRepository, IFileService fileService, ISystemClock clock)
        {
            _fileRepository = fileRepository;
            _fileService = fileService;
            _clock = clock;
        }

        public async Task<string> Handle(CreateMediaFileCommand request, CancellationToken cancellationToken)
        {
            var fileItem = await _fileRepository.LoadByIdAsync(request.FileId!);
            if (fileItem != null)
            {
                return request.FileId!;
            }
            using (var stream = request.FormFile!.OpenReadStream())
            {
                var fileUrl = await _fileService.UploadFileToCloudStorage(stream, request.FormFile.ContentType, $"{request.UserId}/{request.FileId}");
                await _fileRepository.CreateAsync(new File
                {
                    Id = request.FileId!,
                    FileType = request.FileType,
                    FileName = request.FormFile!.FileName,
                    FileSize = (int)request.FormFile!.Length,
                    Width = request.Width,
                    Height = request.Height,
                    UserId = request.UserId,
                    FileContentType = request.FormFile.ContentType,
                    CreatedDate = _clock.UtcNow,
                    ModifiedDate = _clock.UtcNow,
                });
            }

            return request.FileId!;
        }
    }
}
