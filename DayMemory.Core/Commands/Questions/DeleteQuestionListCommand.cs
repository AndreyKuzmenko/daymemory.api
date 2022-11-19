using MediatR;

namespace DayMemory.Core.Commands
{
    public class DeleteQuestionListCommand : IRequest
    {
        public string? QuestionListId { get; set; }
    }
}