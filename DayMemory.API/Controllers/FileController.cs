using DayMemory.Core.Commands.Files;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Services;
//using DayMemory.Web.Components.Services;
using MediatR;
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
        private readonly IMediator _mediator;

        public FileController(IFileRepository fileRepository, IFileService fileService, IUrlResolver urlResolver, IMediator mediator)
        {
            _fileRepository = fileRepository;
            _fileService = fileService;
            _urlResolver = urlResolver;
            _mediator = mediator;
        }

        [Route("api/files/{fileId}")]
        [HttpHead]
        public async Task<ActionResult> CheckIfFileExists([FromRoute] string fileId)
        {
            var fileExists = await _fileRepository.ExistsAsync(fileId);
            if (!fileExists)
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Route("api/files/media/{fileId}")]
        public async Task<ActionResult> UploadMedia([FromRoute] string fileId, [FromForm] int width, [FromForm] int height, [FromForm] FileType fileType, [FromForm] IFormFile? file, CancellationToken ct)
        {
            if (file == null)
            {
                return BadRequest();
            }
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

            var id = await _mediator.Send(command, ct);
            var query = new GetFileQuery { FileId = id, UserId = User.Identity!.Name! };
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
    }
}