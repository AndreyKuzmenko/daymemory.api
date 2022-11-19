using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;

namespace DayMemory.Core.CommandHandlers
{
    internal class CreateQuestionListCommandHandler : IRequestHandler<CreateQuestionListCommand, string>
    {
        private readonly IQuestionListRepository _questionListRepository;
        private readonly ISystemClock _clock;

        public CreateQuestionListCommandHandler(IQuestionListRepository questionListRepository, ISystemClock clock)
        {
            this._questionListRepository = questionListRepository;
            _clock = clock;
        }

        public async Task<string> Handle(CreateQuestionListCommand request, CancellationToken cancellationToken)
        {
            var item = new QuestionList()
            {
                Id = StringUtils.GenerateUniqueString(),
                UserId = request.UserId!,
                OrderRank = request.OrderRank,
                Text = request.Text ?? "",
                CreatedDate = _clock.UtcNow,
                ModifiedDate = _clock.UtcNow,
            };

            foreach (var question in request.Questions)
            {
                var q = new Question()
                {
                    Id = StringUtils.GenerateUniqueString(),
                    QuestionListId = item.Id,
                    Text = question ?? "",
                    CreatedDate = _clock.UtcNow,
                    ModifiedDate = _clock.UtcNow,
                };
                item.Questions.Add(q);
            }

            await _questionListRepository.CreateAsync(item, cancellationToken);

            return item.Id;
        }
    }
}
