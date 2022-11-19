using DayMemory.Core.Commands;
using DayMemory.Core.Interfaces.Repositories;
using MediatR;

namespace DayMemory.Core.CommandHandlers
{
    public class DeleteQuestionListByIdCommandHandler : IRequestHandler<DeleteQuestionListCommand, Unit>
    {
        private readonly IQuestionListRepository _questionListRepository;

        public DeleteQuestionListByIdCommandHandler(IQuestionListRepository questionListRepository)
        {
            this._questionListRepository = questionListRepository;
        }

        public async Task<Unit> Handle(DeleteQuestionListCommand request, CancellationToken cancellationToken)
        {
            await _questionListRepository.DeleteByIdAsync(request.QuestionListId!);
            return Unit.Value;
        }
    }
}