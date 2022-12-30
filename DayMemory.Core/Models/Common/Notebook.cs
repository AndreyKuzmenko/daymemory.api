using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public enum SortingType
    {
        ByCreatedDate = 0,

        ByModifiedDate = 1
    }
    
    public class Notebook : BaseEntity
    {
        public string? Title { get; set; }

        public virtual List<NoteItem> Notes { get; set; } = new List<NoteItem>();

        public string? UserId { get; set; }

        public virtual User? User { get; set; }

        public int OrderRank { get; set; }

        public bool ShowInReview { get; set; }

        public bool IsDeleted { get; set; }

        public SortingType SortingType { get; set; }
    }
}
