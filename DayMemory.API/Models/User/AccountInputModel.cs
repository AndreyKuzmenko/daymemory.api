using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class AccountInputModel
    {
        [Required, MaxLength(128)]
        public string? FirstName { get; set; }

        [Required, MaxLength(128)]
        public string? LastName { get; set; }
    }
}
