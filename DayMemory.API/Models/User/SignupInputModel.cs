using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class SignupInputModel
    {
        [Required(ErrorMessage = "First name is required."), MaxLength(128)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required."), MaxLength(128)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "E-mail is required."), MaxLength(128), EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required."), MaxLength(128)]
        public string? Password { get; set; }
    }
}
