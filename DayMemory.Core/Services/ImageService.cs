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
        public Tuple<byte[], int, int> ResizeImage(byte[] bytes, int maxSize, int quality)
        {
            using (SKImage img = SKImage.FromEncodedData(bytes))
            {
                using (SKBitmap sourceBitmap = SKBitmap.FromImage(img))
                {
                    var size = GetScaleSize(sourceBitmap, maxSize);

                    using (SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(size.Width, size.Height), SKFilterQuality.High))
                    {
                        using (SKImage scaledImage = SKImage.FromBitmap(scaledBitmap))
                        {
                            using (SKData data = scaledImage.Encode(SKEncodedImageFormat.Jpeg, quality))
                            {
                                return new Tuple<byte[], int, int>(data.ToArray(), size.Height, size.Width);
                            }
                        }
                    }
                }
            }
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
