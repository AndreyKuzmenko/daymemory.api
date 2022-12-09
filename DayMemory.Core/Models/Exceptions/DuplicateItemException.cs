using System;

namespace DayMemory.Core.Models.Exceptions
{
    [Serializable]
    public class DuplicateItemException : Exception
    {
        public string ResourceId { get; set; }
        public DuplicateItemException(string resourceId) : base($"Duplicate Item. Resource Id: '{resourceId}'")
        {
            ResourceId = resourceId;
        }

    }
}
