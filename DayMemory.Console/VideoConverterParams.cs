using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Console
{
    public class VideoConverterParams
    {
        public required string ConverterContainerName { get; set; }

        public required string ContainerName { get; set; }

        public required string UserId { get; set; }

        public required string FileId { get; set; }

        public required string InputFileName { get; set; }

        public required string OutputFileName { get; set; }
    }
}
