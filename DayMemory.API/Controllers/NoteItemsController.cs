using Google.Cloud.Firestore;
using DayMemory.Core.Commands;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayMemory.API.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    public class NoteItemsController : ControllerBase
    {
        private readonly ILogger<NoteItemsController> _logger;
        private readonly IMediator _mediator;

        public NoteItemsController(ILogger<NoteItemsController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }

        [HttpGet("{noteItemId}")]
        public async Task<IActionResult> GetNoteItem(string noteItemId, CancellationToken ct)
        {
            var item = await _mediator.Send(new GetNoteItemQuery() { UserId = User.Identity!.Name, NoteItemId = noteItemId }, ct);
            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes([FromQuery] GetAllNoteItemsQuery query, CancellationToken ct)
        {
            query.UserId = User.Identity!.Name;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromBody] CreateNoteItemCommand command, CancellationToken ct)
        {
            var userId = User.Identity!.Name;
            command.UserId = userId;

            var itemId = await _mediator.Send(command);
            var query = new GetNoteItemQuery { NoteItemId = itemId, UserId = userId };
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpPut("{noteId}")]
        public async Task<ActionResult> Put(string noteId, [FromBody] UpdateNoteItemCommand command, CancellationToken ct)
        {
            command.NoteId = noteId;
            await _mediator.Send(command);
            var query = new GetNoteItemQuery { NoteItemId = noteId, UserId = User.Identity!.Name };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpDelete("{noteId}")]
        public async Task<ActionResult> DeleteTopic(string noteId, CancellationToken ct)
        {
            await _mediator.Send(new DeleteNoteItemCommand { NoteItemId = noteId }, ct);
            return Ok();
        }
    }
}