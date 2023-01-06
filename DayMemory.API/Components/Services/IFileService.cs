using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DayMemory.Core.Settings;

namespace DayMemory.Web.Components.Services
{
    public interface IFileService
    {
        Task<string> UploadFileToCloudStorage(Stream stream, string contentType, string fileId);
    }
}