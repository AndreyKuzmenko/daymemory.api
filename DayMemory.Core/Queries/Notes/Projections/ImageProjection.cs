using DayMemory.Core.Queries.Tags.Projections;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;

namespace DayMemory.Core.Queries.Projections
{
    public class ImageProjection
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}