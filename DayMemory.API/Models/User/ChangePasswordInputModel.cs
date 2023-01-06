using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class ChangePasswordInputModel
    {
        [Required, MaxLength(128)]
        public required string CurrentPassword { get; set; }

        [Required, MaxLength(128)]
        public required string NewPassword { get; set; }

        [Required, MaxLength(128)]
        public required string ConfirmationPassword { get; set; }
    }
}
