using DayMemory.Core.Models.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Console.Models
{
    public class FileProjection
    {
        public required string Id { get; set; }

        public string? Name { get; set; }

        public required string OriginalUrl { get; set; }

        public required string ResizedUrl { get; set; }

        public int FileSize { get; set; }

        public FileType FileType { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}
