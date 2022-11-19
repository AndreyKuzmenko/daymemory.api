using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public class QuestionList : BaseEntity
    {
        public string? Text { get; set; }

        public virtual List<Question> Questions { get; set; } = new List<Question>();

        public string? UserId { get; set; }

        public int OrderRank { get; set; }

        public virtual User? User { get; set; }
    }
}
