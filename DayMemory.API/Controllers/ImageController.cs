using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using DayMemory.Web.Components.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace DayMemory.API.Controllers
{
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        private readonly IImageService _imageService;

        private readonly IUrlResolver _urlResolver;

        public ImageController(IImageRepository imageRepository, IImageService imageService, IUrlResolver urlResolver)
        {
            _imageRepository = imageRepository;
            _imageService = imageService;
            this._urlResolver = urlResolver;
        }

        [Route("api/images/{source}/{imageId}")]
        [HttpHead]
        public async Task<ActionResult<ImageDto>> CheckIfImageExists([FromRoute] string imageId)
        {
            var imageExists = await _imageRepository.ExistsAsync(imageId);
            if (!imageExists)
                return NotFound();

            return Ok();
        }

        
        [HttpPost]
        [Route("api/images/{source}/{imageId}")]
        public async Task<ActionResult<ImageDto>> Upload([FromRoute] string source, [FromRoute] string imageId, [FromForm] IFormFile file)
        {
            var userId = User.Identity!.Name!;
            //TODO: Change to COMMAND
            if (!Enum.TryParse(source, true, out ImageSource imageSource))
            {
                return BadRequest(ModelState);
            }

            if (file == null)
            {
                return BadRequest();
            }

            //TODO: Inconsistency with other APIs. Should throw exception if file exists
            var image = await _imageRepository.LoadByIdAsync(imageId);
            if (image != null)
            {
                return new ImageDto
                {
                    Id = imageId,
                    Name = file.FileName,
                    Url = _urlResolver.GetImageUrlTemplate(imageSource, userId),
                    Height = image.Height,
                    Width = image.Width
                };
            }


            var imageUrl = await _imageService.UploadFileToCloudStorage(file.OpenReadStream(), file.ContentType, $"{userId}/{imageId}", source);

            using (var skImage = SKImage.FromEncodedData(file.OpenReadStream()))
            {
                await SaveImage(imageSource, file.FileName, file.ContentType, (int)file.Length, skImage.Width, skImage.Height, imageId, userId);

                return new ImageDto
                {
                    Id = imageId,
                    Name = file.FileName,
                    Url = imageUrl,
                    Height = skImage.Height,
                    Width = skImage.Width
                };
            }
        }

        private async Task SaveImage(ImageSource imageSource, string fileName, string contentType, int size, int width, int height, string newId, string userId)
        {
            await _imageRepository.CreateAsync(new Image
            {
                Id = newId,
                FileName = fileName,
                FileSize = size,
                Width = width,
                Height = height,
                Source = imageSource,
                UserId = userId,
                FileContentType = contentType,
                CreatedDate = DateTimeOffset.UtcNow,
                ModifiedDate = DateTimeOffset.UtcNow,
            });
        }
    }
}