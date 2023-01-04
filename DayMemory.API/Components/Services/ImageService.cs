using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DayMemory.Core.Settings;

namespace DayMemory.Web.Components.Services
{
    public interface IFileService
    {
        Task<string> UploadFileToCloudStorage(Stream stream, string contentType, string fileId);
    }

    public class AzureFileService : IFileService
    {
        private readonly IConfiguration configuration;
        private readonly UrlSettings _urlSettings;

        public AzureFileService(IConfiguration configuration, UrlSettings urlSettings)
        {
            this.configuration = configuration;
            this._urlSettings = urlSettings;
        }
        public async Task<string> UploadFileToCloudStorage(Stream stream, string contentType, string fileId)
        {
            var containerName = "note-images";
            var blobContainerClient = new BlobContainerClient(_urlSettings.StorageConnectionString, containerName);

            BlobClient blob = blobContainerClient.GetBlobClient(fileId);

            await blob.UploadAsync(stream, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType } });

            return blob.Uri.AbsoluteUri;
        }
    }
}