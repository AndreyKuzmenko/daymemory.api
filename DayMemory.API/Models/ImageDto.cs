using System;

namespace DayMemory.Core.Models
{
    public class ImageDto
    {
        public string? Id { get; set; }
        
        public string? Name { get; set; }

        public string? Url { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}
