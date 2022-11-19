using DayMemory.Core.Queries.Categories.Projections;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Queries.Projections;
using MediatR;

namespace DayMemory.Core.Queries
{
    public class GetAllQuestionListQuery : IRequest<IList<QuestionListProjection>>
    {
        public string? UserId { get; set; }
    }
}