using System;

namespace DayMemory.Core.Models.Common
{
    public abstract class BaseEntity
    {
        public string? Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
