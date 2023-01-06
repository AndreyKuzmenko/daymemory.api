using System;
using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class SocialSignupInputModel
    {
        [Required(ErrorMessage = "Id is required."), MaxLength(128)]
        public required string Id { get; set; }
        
        public required string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ImageUrl { get; set; }

        public required string ProviderType { get; set; }
    }
}
