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
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
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

            //string tempPath = "\\wwwroot";
            //var exeFolder = Path.Combine(tempPath, "ffmpeg");
            //var inputFileFolder = Path.Combine(tempPath, "ffmpeg", request.FileId!);
            //var outputFileFolder = Path.Combine(tempPath, "ffmpeg", request.FileId!);

            //if (!Directory.Exists(inputFileFolder))
            //{
            //    Directory.CreateDirectory(inputFileFolder);
            //}

            //if (!Directory.Exists(exeFolder))
            //{
            //    Directory.CreateDirectory(exeFolder);
            //}

            //FFmpeg.SetExecutablesPath(exeFolder);
            //await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, exeFolder);



            using (var stream = request.FormFile.OpenReadStream())
            {

                //var filePath = Path.Combine(inputFileFolder, request.FormFile.FileName);
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}
                //var outputPath = Path.Combine(outputFileFolder, $"output.mp4");

                //if (System.IO.File.Exists(outputPath))
                //{
                //    System.IO.File.Delete(outputPath);
                //}

                //using (var fileStream = System.IO.File.Create(filePath))
                //{
                //    stream.Seek(0, SeekOrigin.Begin);
                //    await stream.CopyToAsync(fileStream);
                //}

                //IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                //var c = await FFmpeg.Conversions.FromSnippet.ToMp4(filePath, outputPath);
                //await c.Start();

                //using (var fileStream = System.IO.File.OpenRead(filePath))
                //{
                var fileUrl = await _fileService.UploadFileToCloudStorage(stream/*fileStream*/, request.FormFile.ContentType, $"{request.UserId}/{request.FileId}");
                await _fileRepository.CreateAsync(new File
                {
                    Id = request.FileId!,
                    FileType = request.FileType,
                    FileName = Path.GetFileNameWithoutExtension(request.FormFile.FileName) + ".mp4",
                    FileSize = (int)request.FormFile.Length,
                    Width = request.Width,
                    Height = request.Height,
                    UserId = request.UserId,
                    FileContentType = request.FormFile.ContentType,
                    CreatedDate = _clock.UtcNow,
                    ModifiedDate = _clock.UtcNow,
                });
                //}
                //stream.Seek(0, SeekOrigin.Begin);
            }

            return request.FileId!;
        }
    }
}
