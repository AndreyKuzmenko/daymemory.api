using DayMemory.Core.Models.Common;
using Microsoft.VisualBasic;

namespace DayMemory.Core.Models.Personal
{
    public class Tag : BaseEntity
    {
        public required string Text { get; set; }

        public required string UserId { get; set; }

        public virtual User? User { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEncrypted { get; set; }

        public int OrderRank { get; set; }
    }
}
