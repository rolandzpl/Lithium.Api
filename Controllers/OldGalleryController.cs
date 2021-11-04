using System.Text.RegularExpressions;
using Lithium.Api.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace Lithium.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OldGalleryController : ControllerBase
{
    private readonly GalleryConfiguration configuration;
    private readonly ILogger<OldGalleryController> logger;

    public OldGalleryController(GalleryConfiguration configuration, ILogger<OldGalleryController> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    [HttpGet("/legacy/galleries")]
    public IEnumerable<GalleryInfoDto> GetGalleries() =>
        Directory
            .GetDirectories(configuration.RootDirectory)
            .Select(_ => new GalleryInfoDto
            {
                GalleryId = Path.GetFileName(_),
                Title = ""
            });

    [HttpGet("/legacy/galleries/{galleryId}")]
    public IEnumerable<ImageRefDto> Get(string galleryId) =>
        Directory
            .GetFiles(Path.Combine(configuration.RootDirectory, galleryId), "*.jpg")
            .Where(IsNotThumbnail)
            .Select(_ =>
            {
                var imageInfo = Image.Identify(_);
                logger.LogDebug($"Image: {_}, height: {imageInfo.Height}, width: {imageInfo.Width}");
                var originalFileName = Path.GetFileName(_);
                var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(_)}_thumb{Path.GetExtension(_)}";
                return new ImageRefDto
                {
                    Thumbnail = $"{configuration.BaseUrl}/{galleryId}/{thumbnailFileName}",
                    Src = $"{configuration.BaseUrl}/{galleryId}/{originalFileName}",
                    Width = imageInfo.Width,
                    Height = imageInfo.Height
                };
            });

    private bool IsNotThumbnail(string path)
    {
        return !Regex.Match(path, ".+_thumb\\..+").Success;
    }
}
