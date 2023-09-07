using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public class NoteItem : BaseEntity
    {
        public string? Text { get; set; }

        public DateTimeOffset Date { get; set; }

        public string? NotebookId { get; set; }

        public virtual Notebook? Notebook { get; set; }

        public string? LocationId { get; set; }

        public virtual Location? Location { get; set; }

        public virtual List<NoteFile> Files { get; set; } = new List<NoteFile>();

        public virtual List<NoteTag> Tags { get; set; } = new List<NoteTag>();

        public string? UserId { get; set; }

        public virtual User? User { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEncrypted { get; set; }

        public void SetTags(string[] tagIds)
        {
            Tags.Clear();
            foreach (var tagId in tagIds)
            {
                var dt = new NoteTag
                {
                    Id = StringUtils.GenerateUniqueString(),
                    TagId = tagId,
                    NoteItemId = Id,
                    CreatedDate = DateTimeOffset.UtcNow,
                    ModifiedDate = DateTimeOffset.UtcNow,
                };
                Tags.Add(dt);
            }
        }

        public void SetFiles(string[] fileIds)
        {
            Files.Clear();
            int i = 0;
            foreach (var fileId in fileIds)
            {
                var dt = new NoteFile
                {
                    Id = StringUtils.GenerateUniqueString(),
                    FileId = fileId,
                    NoteItemId = Id,
                    OrderRank = i++,
                    CreatedDate = DateTimeOffset.UtcNow,
                    ModifiedDate = DateTimeOffset.UtcNow,
                };

                Files.Add(dt);
            }
        }
    }
}
