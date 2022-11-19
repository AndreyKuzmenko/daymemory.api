using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class ChangePasswordInputModel
    {
        [Required, MaxLength(128)]
        public string? CurrentPassword { get; set; }

        [Required, MaxLength(128)]
        public string? NewPassword { get; set; }

        [Required, MaxLength(128)]
        public string? ConfirmationPassword { get; set; }
    }
}
