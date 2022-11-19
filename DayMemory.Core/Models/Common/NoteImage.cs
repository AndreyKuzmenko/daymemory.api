using System;
using DayMemory.Core.Models.Common;

namespace DayMemory.Core.Models.Personal
{
    public class NoteImage : BaseEntity
    {
        public virtual string? ImageId { get; set; }

        public virtual Image? Image { get; set; }

        public virtual string? NoteItemId { get; set; }

        public virtual NoteItem? NoteItem { get; set; }

        public int OrderRank { get; set; }
    }
}
