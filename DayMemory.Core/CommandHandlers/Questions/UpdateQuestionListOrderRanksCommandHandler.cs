using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace DayMemory.Core.CommandHandlers
{
    internal class UpdateQuestionListOrderRanksCommandHandler : AsyncRequestHandler<UpdateQuestionListOrderRanksCommand>
    {
        private readonly IQuestionListRepository _questionListRepository;
        private readonly ISystemClock _clock;

        public UpdateQuestionListOrderRanksCommandHandler(IQuestionListRepository questionListRepository, ISystemClock clock)
        {
            this._questionListRepository = questionListRepository;
            _clock = clock;
        }

        protected override async Task Handle(UpdateQuestionListOrderRanksCommand request, CancellationToken cancellationToken)
        {
            foreach (var questionList in request.QuestionLists)
            {
                var item = await _questionListRepository.LoadByIdAsync(questionList.QuestionListId!, cancellationToken);
                if (item == null)
                {
                    throw new ResourceNotFoundException("Question List is not found", questionList.QuestionListId!);
                }
                item.ModifiedDate = _clock.UtcNow;
                item.OrderRank = questionList.OrderRank;
                await _questionListRepository.UpdateAsync(item, cancellationToken);
            }
        }
    }
}
