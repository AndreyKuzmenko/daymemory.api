using DayMemory.Core.Models.Common;

namespace DayMemory.Core.Models.Personal
{
    public class Location : BaseEntity
    {
        public virtual NoteItem? NoteItem { get; set; }

        public string? Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
