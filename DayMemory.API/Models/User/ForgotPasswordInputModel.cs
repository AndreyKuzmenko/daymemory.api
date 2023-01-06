using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class ForgotPasswordInputModel
    {
        [Required, MaxLength(128), EmailAddress]
        public required string Email { get; set; }
    }
}
