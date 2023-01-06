using DayMemory.Core.Commands;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayMemory.API.Controllers
{
    [ApiController]
    [Route("api/tags")]
    [Authorize]
    public class TagController : ControllerBase
    {

        private readonly ILogger<TagController> _logger;
        private readonly IMediator _mediator;

        public TagController(ILogger<TagController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }

        [HttpGet("{tagId}")]
        public async Task<IActionResult> GetTag(string tagId, CancellationToken ct)
        {
            var items = await _mediator.Send(new GetTagQuery() { UserId = User.Identity!.Name!, TagId = tagId }, ct);
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags([FromQuery] GetAllTagsQuery query, CancellationToken ct)
        {
            query.UserId = User.Identity!.Name!;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateTagCommand command, CancellationToken ct)
        {
            var userId = User.Identity!.Name!;
            command.UserId = userId;

            var item = await _mediator.Send(new GetTagQuery() { UserId = User.Identity!.Name!, TagId = command.TagId }, ct);
            if (item != null)
            {
                throw new DuplicateItemException(command.TagId!);
            }

            var itemId = await _mediator.Send(command);
            var query = new GetTagQuery { TagId = itemId, UserId = userId };
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpPut("{tagId}")]
        public async Task<ActionResult> Put(string tagId, [FromBody] UpdateTagCommand command, CancellationToken ct)
        {
            command.TagId = tagId;
            command.UserId = User.Identity!.Name!;
            await _mediator.Send(command);
            var query = new GetTagQuery { TagId = tagId, UserId = User.Identity!.Name! };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpDelete("{tagId}")]
        public async Task<ActionResult> Delete(string tagId, CancellationToken ct)
        {
            var command = new DeleteTagCommand
            {
                TagId = tagId,
                UserId = User.Identity!.Name!
            };

            await _mediator.Send(command, ct);

            return Ok();
        }
    }
}