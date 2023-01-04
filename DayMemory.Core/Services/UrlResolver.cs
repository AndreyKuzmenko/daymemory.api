using System;
using DayMemory.Core.Settings;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using Microsoft.Extensions.Configuration;

namespace DayMemory.Core.Services
{
    public interface IUrlResolver
    {
        string GetFileUrlTemplate(string userId);
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

        public string GetFileUrlTemplate(string userId)
        {
            return $"{_urlSettings.BlobStorageRootUrl}/note-images/{userId}/{{0}}";
        }
    }
}
