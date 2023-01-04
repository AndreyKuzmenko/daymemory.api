using System;
using DayMemory.Core.Models.Common;

namespace DayMemory.Core.Models.Personal
{
    public class NoteFile : BaseEntity
    {
        public virtual string? FileId { get; set; }

        public virtual File? File { get; set; }

        public virtual string? NoteItemId { get; set; }

        public virtual NoteItem? NoteItem { get; set; }

        public int OrderRank { get; set; }
    }
}
