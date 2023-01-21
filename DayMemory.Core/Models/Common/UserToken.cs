using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;

namespace DayMemory.Core.Models.Personal
{
    public class UserToken : BaseEntity
    {
        public required string RefreshToken { get; set; }

        public required string UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
