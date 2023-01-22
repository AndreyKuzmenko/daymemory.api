using System;
using DayMemory.Core.Settings;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using Microsoft.Extensions.Configuration;

namespace DayMemory.Core.Services
{
    public interface IUrlResolver
    {
        string GetOriginalFileUrlTemplate(string userId);
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

        public string GetOriginalFileUrlTemplate(string userId)
        {
            var containerName = _configuration["FileStorage:Container"];
            return $"{_urlSettings.BlobStorageRootUrl}/{containerName}/{userId}/{{0}}/original";
        }
    }
}
