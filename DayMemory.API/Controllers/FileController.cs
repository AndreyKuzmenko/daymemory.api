using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using DayMemory.Web.Components.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace DayMemory.API.Controllers
{
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;

        private readonly IFileService _fileService;

        private readonly IUrlResolver _urlResolver;

        public FileController(IFileRepository fileRepository, IFileService fileService, IUrlResolver urlResolver)
        {
            _fileRepository = fileRepository;
            _fileService = fileService;
            this._urlResolver = urlResolver;
        }

        [Route("api/files/{fileId}")]
        [HttpHead]
        public async Task<ActionResult<FileDto>> CheckIfFileExists([FromRoute] string fileId)
        {
            var fileExists = await _fileRepository.ExistsAsync(fileId);
            if (!fileExists)
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Route("api/files/{source}/{fileId}")]
        public async Task<ActionResult<FileDto>> Upload([FromRoute] string fileId, [FromForm] IFormFile file)
        {
            var userId = User.Identity!.Name!;
            //TODO: Change to COMMAND
            if (file == null)
            {
                return BadRequest();
            }

            //TODO: Inconsistency with other APIs. Should throw exception if file exists
            var fileItem = await _fileRepository.LoadByIdAsync(fileId);
            if (fileItem != null)
            {
                return new FileDto
                {
                    Id = fileId,
                    Name = file.FileName,
                    Url = _urlResolver.GetFileUrlTemplate( userId),
                    Height = fileItem.Height,
                    Width = fileItem.Width
                };
            }

            var fileUrl = await _fileService.UploadFileToCloudStorage(file.OpenReadStream(), file.ContentType, $"{userId}/{fileId}");

            using (var skImage = SKImage.FromEncodedData(file.OpenReadStream()))
            {
                await SaveFile(file.FileName, file.ContentType, (int)file.Length, skImage.Width, skImage.Height, fileId, userId);

                return new FileDto
                {
                    Id = fileId,
                    Name = file.FileName,
                    Url = fileUrl,
                    Height = skImage.Height,
                    Width = skImage.Width
                };
            }
        }

        private async Task SaveFile(string fileName, string contentType, int size, int width, int height, string newId, string userId)
        {
            await _fileRepository.CreateAsync(new Image
            {
                Id = newId,
                FileName = fileName,
                FileSize = size,
                Width = width,
                Height = height,
                UserId = userId,
                FileContentType = contentType,
                CreatedDate = DateTimeOffset.UtcNow,
                ModifiedDate = DateTimeOffset.UtcNow,
            });
        }
    }
}