using System;
using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class SocialSignupInputModel
    {
        [Required(ErrorMessage = "Id is required."), MaxLength(128)]
        public string? Id { get; set; }
        
        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ImageUrl { get; set; }

        public string? ProviderType { get; set; }
    }
}
