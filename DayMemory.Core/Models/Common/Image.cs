using DayMemory.Core.Models.Common;

namespace DayMemory.Core.Models.Personal
{
    public enum FileSource
    {
        Note = 1,

        Profile = 2
    }

    public class File : BaseEntity
    {
        public string? FileName { get; set; }

        public string? FileContentType { get; set; }

        public int FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public virtual User? User { get; set; }

        public string? UserId { get; set; }
        
        public File()
        {
        }
    }
}
