using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class RestorePasswordInputModel
    {
        [Required, MaxLength(128)]
        public required string Password { get; set; }

        [Required, MaxLength(128), EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }
    }
}
