using System.ComponentModel.DataAnnotations;

namespace DayMemory.API.Models.User
{
    public class TokenModel
    {
        [Required]
        public required string AccessToken { get; set; }

        [Required]
        public required string RefreshToken { get; set; }
    }
}
