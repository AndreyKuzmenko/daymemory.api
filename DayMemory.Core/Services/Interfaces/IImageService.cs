using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DayMemory.Core.Settings;
using SkiaSharp;
using System.Drawing;

namespace DayMemory.Core.Services.Interfaces
{
    public class ResizedImage {
        public required Stream Stream { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }

    public interface IImageService
    {
        Task<ResizedImage> ResizeImageAsync(Stream stream, int maxSize, int quality);
    }

}