using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public class Question : BaseEntity
    {
        public string? Text { get; set; }

        public string? QuestionListId { get; set; }

        public virtual QuestionList? QuestionList { get; set; }
    }
}
