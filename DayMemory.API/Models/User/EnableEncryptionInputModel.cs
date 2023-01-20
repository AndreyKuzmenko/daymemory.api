using System.ComponentModel.DataAnnotations;

namespace DayMemory.Web.Api.Models
{
    public class EnableEncryptionInputModel
    {
        [Required, MaxLength(128)]
        public required string EncryptedText { get; set; }
    }
}
