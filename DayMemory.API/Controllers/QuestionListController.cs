using Google.Cloud.Firestore;
using DayMemory.Core.Commands;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DayMemory.API.Controllers
{
    [ApiController]
    [Route("api/question-lists")]
    [Authorize]
    public class QuestionListController : ControllerBase
    {

        private readonly ILogger<QuestionListController> _logger;
        private readonly IMediator _mediator;

        public QuestionListController(ILogger<QuestionListController> logger, IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }


        [HttpGet("{questionListId}")]
        public async Task<IActionResult> Get(string questionListId, CancellationToken ct)
        {
            var items = await _mediator.Send(new GetQuestionListQuery() { UserId = User.Identity!.Name, QuestionListId = questionListId }, ct);
            return Ok(items);
        }

        [HttpPut("order-ranks")]
        public async Task<ActionResult> UpdateOrderRanks([FromBody] QuestionListItem[] items, CancellationToken ct)
        {
            await _mediator.Send(new UpdateQuestionListOrderRanksCommand() { QuestionLists = items }, ct);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateQuestionListCommand command, CancellationToken ct)
        {
            var userId = User.Identity!.Name;
            command.UserId = userId;

            var itemId = await _mediator.Send(command);
            var query = new GetQuestionListQuery { QuestionListId = itemId, UserId = userId };
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpPut("{questionListId}")]
        public async Task<ActionResult> Put(string questionListId, [FromBody] UpdateQuestionListCommand command, CancellationToken ct)
        {
            command.QuestionListId = questionListId;
            await _mediator.Send(command);
            var query = new GetQuestionListQuery { QuestionListId = questionListId, UserId = User.Identity!.Name };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }


        [HttpDelete("{questionListId}")]
        public async Task<ActionResult> Delete(string questionListId, CancellationToken ct)
        {
            var command = new DeleteQuestionListCommand
            {
                QuestionListId = questionListId
            };

            await _mediator.Send(command, ct);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var query = new GetAllQuestionListQuery();
            query.UserId = User.Identity!.Name;
            var items = await _mediator.Send(query, ct);
            return Ok(items);
        }
    }
}