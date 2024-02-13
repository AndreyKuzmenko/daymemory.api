using DayMemory.Core;
using DayMemory.Core.Commands.Files;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Services;
using DayMemory.Core.Services.Interfaces;
//using DayMemory.Web.Components.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Text.Json;

namespace DayMemory.API.Controllers
{
    [Authorize]
    public class FileController(IFileRepository fileRepository, IMediator mediator, ILogger<FileController> logger) : ControllerBase
    {
        [Route("api/files/{fileId}")]
        [HttpHead]
        public async Task<ActionResult> CheckIfFileExists([FromRoute] string fileId)
        {
            var fileExists = await fileRepository.ExistsAsync(fileId);
            if (!fileExists)
                return NotFound();

            return Ok();
        }

        [Route("api/files/{fileId}")]
        [HttpGet]
        public async Task<ActionResult> GetFile([FromRoute] string fileId, CancellationToken ct)
        {
            var query = new GetFileQuery { FileId = fileId, UserId = User.Identity!.Name! };
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/files/media")]
        [RequestSizeLimit(Constants.RequestLimits.MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants.RequestLimits.MaxFileSize)]
        public async Task<IActionResult> UploadMedia([FromForm] string fileId, [FromForm] int width, [FromForm] int height, [FromForm] FileType fileType, [FromForm] IFormFile? file, CancellationToken ct)
        {
            if (file == null)
            {
                logger.LogInformation("No file was uploaded");
                return BadRequest();
            }

            if (fileType == FileType.Unknown)
            {
                logger.LogInformation("Unknown file type");
                return BadRequest("File type is unknown.");
            }

            logger.LogInformation("Uploading file: {0}, size: {1}", file.FileName, file.Length);

            var userId = User.Identity!.Name!;
            var command = new CreateMediaFileCommand()
            {
                FormFile = file,
                FileId = fileId,
                FileType = fileType,
                Width = width,
                Height = height,
                UserId = userId
            };

            var id = await mediator.Send(command, ct);            

            var query = new GetFileQuery { FileId = id, UserId = User.Identity!.Name! };
            var result = await mediator.Send(query, ct);

            logger.LogInformation("File uploaded: {0}", result!.OriginalUrl);

            return Ok(result);
        }
    }
}