using System;

namespace DayMemory.Core.Models.Common
{
    public abstract class BaseEntity
    {
        public required string Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset ModifiedDate { get; set; }
    }
}
