using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class RestorePasswordInputModel
    {
        [Required, MaxLength(128)]
        public string? Password { get; set; }

        [Required, MaxLength(128), EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
