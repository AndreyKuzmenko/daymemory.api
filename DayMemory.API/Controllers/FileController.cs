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

        [Route("api/files/{fileId}")]
        [HttpGet]
        public async Task<ActionResult> GetFile([FromRoute] string fileId, CancellationToken ct)
        {
            var query = new GetFileQuery { FileId = fileId, UserId = User.Identity!.Name! };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/files/media")]
        public async Task<ActionResult> UploadMedia([FromForm] CreateMediaFileCommand command, [FromForm] IFormFile file, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (command.FileType == FileType.Unknown)
            {
                return BadRequest("File type is unknown.");
            }

            var userId = User.Identity!.Name!;
            command.UserId = userId;
            command.FormFile = file;

            var id = await _mediator.Send(command, ct);
            var query = new GetFileQuery { FileId = id, UserId = User.Identity!.Name! };
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
    }
}