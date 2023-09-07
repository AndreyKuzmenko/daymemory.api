using System;
using DayMemory.Core.Models.Common;

namespace DayMemory.Core.Models.Personal
{
    public class NoteTag : BaseEntity
    {
        public virtual string? TagId { get; set; }

        public virtual Tag? Tag { get; set; }

        public virtual string? NoteItemId { get; set; }

        public virtual NoteItem? NoteItem { get; set; }
    }
}
