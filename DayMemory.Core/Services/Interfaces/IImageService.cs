using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DayMemory.Core.Settings;
using SkiaSharp;
using System.Drawing;

namespace DayMemory.Core.Services.Interfaces
{
    public interface IImageService
    {
        Tuple<byte[], int, int> ResizeImage(byte[] bytes, int maxSize, int quality);
    }

}