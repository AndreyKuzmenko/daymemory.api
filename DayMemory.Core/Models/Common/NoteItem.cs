using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public class NoteItem : BaseEntity
    {
        public string? Text { get; set; }

        public DateTimeOffset Date { get; set; }

        public string? LocationId { get; set; }

        public virtual Location? Location { get; set; }

        public virtual List<NoteImage> Images { get; set; } = new List<NoteImage>();

        public string? UserId { get; set; }

        public virtual User? User { get; set; }

        public bool IsDeleted { get; set; }

        public void SetImages(string[] imageIds)
        {
            Images.Clear();
            int i = 0;
            foreach (var imageId in imageIds)
            {
                var dt = new NoteImage
                {
                    Id = StringUtils.GenerateUniqueString(),
                    ImageId = imageId,
                    NoteItemId = Id,
                    OrderRank = i++,
                    CreatedDate = DateTimeOffset.UtcNow,
                    ModifiedDate = DateTimeOffset.UtcNow,
                };

                Images.Add(dt);
            }
        }
    }
}
