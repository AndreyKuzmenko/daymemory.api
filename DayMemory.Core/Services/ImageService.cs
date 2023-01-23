using DayMemory.Core.Services.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Core.Services
{
    public class ImageService : IImageService
    {
        public async Task<ResizedImage> ResizeImageAsync(Stream stream, int maxSize, int quality)
        {
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(stream);
            using SKImage img = SKImage.FromEncodedData(memoryStream.ToArray());
            using SKBitmap sourceBitmap = SKBitmap.FromImage(img);

            var size = GetScaleSize(sourceBitmap, maxSize);

            using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(size.Width, size.Height), SKFilterQuality.High);
            using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
            using SKData data = scaledImage.Encode(SKEncodedImageFormat.Jpeg, quality);

            var resultStream = new MemoryStream();
            var array = data.ToArray();
            await resultStream.WriteAsync(array);

            return new ResizedImage() { Stream = resultStream, Width = size.Width, Height = size.Height };
        }

        private Size GetScaleSize(SKBitmap bitmap, decimal max)
        {
            decimal scale = 1;
            var maxLength = Math.Max(bitmap.Width, bitmap.Height);
            if (maxLength > max)
            {
                scale = max / maxLength;
            }
            return new Size((int)Math.Round(bitmap.Width * scale), (int)Math.Round(bitmap.Height * scale));
        }
    }
}
