using System;
using DayMemory.Core.Settings;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using Microsoft.Extensions.Configuration;

namespace DayMemory.Core.Services
{
    public interface IUrlResolver
    {
        string GetImageUrlTemplate(ImageSource source, string userId);
    }

    public class UrlResolver : IUrlResolver
    {
        private readonly IConfiguration _configuration;
        private readonly UrlSettings _urlSettings;

        public UrlResolver(IConfiguration configuration, UrlSettings urlSettings)
        {
            _configuration = configuration;
            this._urlSettings = urlSettings;
        }

        public string GetImageUrlTemplate(ImageSource source, string userId)
        {
            return $"{_urlSettings.BlobStorageRootUrl}/{source.ToString().ToLowerInvariant()}-images/{userId}/{{0}}";
        }
    }
}
