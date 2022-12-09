using DayMemory.Core.Commands;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayMemory.API.Controllers
{
    [ApiController]
    [Route("api/notebooks")]
    [Authorize]
    public class NotebookController : ControllerBase
    {

        private readonly ILogger<NotebookController> _logger;
        private readonly IMediator _mediator;

        public NotebookController(ILogger<NotebookController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }

        [HttpGet("{notebookId}")]
        public async Task<IActionResult> GetNotebook(string notebookId, CancellationToken ct)
        {
            var items = await _mediator.Send(new GetNotebookQuery() { UserId = User.Identity!.Name, NotebookId = notebookId }, ct);
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotebooks([FromQuery] GetAllNotebooksQuery query, CancellationToken ct)
        {
            query.UserId = User.Identity!.Name;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateNotebookCommand command, CancellationToken ct)
        {
            var userId = User.Identity!.Name!;
            command.UserId = userId;
            
            var item = await _mediator.Send(new GetNotebookQuery() { UserId = User.Identity!.Name, NotebookId = command.NotebookId }, ct);
            if (item != null)
            {
                throw new DuplicateItemException(command.NotebookId!);
            }


            var itemId = await _mediator.Send(command);
            var query = new GetNotebookQuery { NotebookId = itemId, UserId = userId };
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpPut("{notebookId}")]
        public async Task<ActionResult> Put(string notebookId, [FromBody] UpdateNotebookCommand command, CancellationToken ct)
        {
            command.NotebookId = notebookId;
            await _mediator.Send(command);
            var query = new GetNotebookQuery { NotebookId = notebookId, UserId = User.Identity!.Name };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpDelete("{notebookId}")]
        public async Task<ActionResult> Delete(string notebookId, CancellationToken ct)
        {
            var command = new DeleteNotebookCommand
            {
                NotebookId = notebookId
            };

            await _mediator.Send(command, ct);

            return Ok();
        }
    }
}