using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Settings;
using Microsoft.Extensions.Configuration;

namespace DayMemory.Core.Services
{
    public class AzureFileService : IFileService
    {
        private readonly IConfiguration configuration;
        private readonly UrlSettings _urlSettings;

        public AzureFileService(IConfiguration configuration, UrlSettings urlSettings)
        {
            this.configuration = configuration;
            this._urlSettings = urlSettings;
        }
        public async Task<string> UploadFileToCloudStorage(Stream stream, string contentType, string filePath)
        {
            var containerName = configuration["FileStorage:Container"];
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ConfigurationException("FileStorage:Container");
            }
            var blobContainerClient = new BlobContainerClient(_urlSettings.StorageConnectionString, containerName);

            BlobClient blob = blobContainerClient.GetBlobClient($"{filePath}/original");

            await blob.UploadAsync(stream, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType } });

            return blob.Uri.AbsoluteUri;
        }
    }
}