using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public class QuestionListProjection
    {
        public string? Id { get; set; }

        public string? Text { get; set; }

        public int OrderRank { get; set; }

        public virtual List<QuestionProjection> Questions { get; set; } = new List<QuestionProjection>();
    }
}
