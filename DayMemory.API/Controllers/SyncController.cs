using DayMemory.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayMemory.API.Controllers
{
    [ApiController]
    [Route("api/sync")]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly ILogger<SyncController> _logger;
        private readonly IMediator _mediator;

        public SyncController(ILogger<SyncController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }

        [HttpGet("notes")]
        public async Task<IActionResult> GetAllNotes([FromQuery] GetSyncNoteItemsQuery query, CancellationToken ct)
        {
            query.UserId = User.Identity!.Name;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }

        [HttpGet("notebooks")]
        public async Task<IActionResult> GetAllNotebooks([FromQuery] GetSyncNotebooksQuery query, CancellationToken ct)
        {
            query.UserId = User.Identity!.Name;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetAllTags([FromQuery] GetSyncTagsQuery query, CancellationToken ct)
        {
            query.UserId = User.Identity!.Name;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }
    }
}