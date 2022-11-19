using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetQuestionListQuery : IRequest<QuestionListProjection>
    {
        public string? QuestionListId { get; set; }

        public string? UserId { get; set; }
    }
}