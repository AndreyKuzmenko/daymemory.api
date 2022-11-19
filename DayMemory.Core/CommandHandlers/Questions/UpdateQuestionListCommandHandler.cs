using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using MediatR;
using Microsoft.Extensions.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace DayMemory.Core.CommandHandlers
{
    internal class UpdateQuestionListCommandHandler : AsyncRequestHandler<UpdateQuestionListCommand>
    {
        private readonly IQuestionListRepository _questionListRepository;
        private readonly ISystemClock _clock;

        public UpdateQuestionListCommandHandler(IQuestionListRepository questionListRepository, ISystemClock clock)
        {
            this._questionListRepository = questionListRepository;
            _clock = clock;
        }

        protected override async Task Handle(UpdateQuestionListCommand request, CancellationToken cancellationToken)
        {
            var item = await _questionListRepository.LoadByIdAsync(request.QuestionListId!, cancellationToken);
            if (item == null)
            {
                throw new InvalidOperationException("Item does not exist");
            }

            item.Text = request.Text ?? "";
            item.ModifiedDate = _clock.UtcNow;
            item.OrderRank = request.OrderRank;

            item.Questions.Clear();

            foreach (var question in request.Questions)
            {
                var q = new Question()
                {
                    Id = StringUtils.GenerateUniqueString(),
                    QuestionListId = item!.Id!,
                    Text = question ?? "",
                    CreatedDate = _clock.UtcNow,
                    ModifiedDate = _clock.UtcNow,
                };
                item.Questions.Add(q);
            }

            await _questionListRepository.UpdateAsync(item!, cancellationToken);
        }
    }
}
