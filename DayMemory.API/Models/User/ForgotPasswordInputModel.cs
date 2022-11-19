using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class ForgotPasswordInputModel
    {
        [Required, MaxLength(128), EmailAddress]
        public string? Email { get; set; }
    }
}
