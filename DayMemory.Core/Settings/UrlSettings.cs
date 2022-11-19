using System.ComponentModel.DataAnnotations;

namespace DayMemory.Core.Settings
{
    public class UrlSettings
    {

        [Required]
        public string? StorageConnectionString { get; set; }

        [Required, Url]
        public string? BlobStorageRootUrl { get; set; }

        [Required, Url]
        public string? RetorePasswordUrlTemplate { get; set; }
    }
}
