using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class CheckEmailInputModel
    {
        [Required(ErrorMessage = "E-mail is required."), MaxLength(128), EmailAddress]
        public required string Email { get; set; }
    }
}
