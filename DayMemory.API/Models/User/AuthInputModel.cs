using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class AuthInputModel
    {
        [Required]
        public string? AccessToken { get; set; }
    }
}
