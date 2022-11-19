using DayMemory.Core.Models.Common;
using Microsoft.VisualBasic;

namespace DayMemory.Core.Models.Personal
{
    public class Tag : BaseEntity
    {
        public string? Text { get; set; }

        public string? UserId { get; set; }

        public virtual User? User { get; set; }

        public bool IsDeleted { get; set; }

        public int OrderRank { get; set; }
    }
}
