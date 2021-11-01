using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace Lithium.Api.Gallery.Controllers;

[ApiController]
[Route("[controller]")]
public class GalleryController : ControllerBase
{
    private readonly GalleryConfiguration configuration;
    private readonly ILogger<GalleryController> logger;

    public GalleryController(GalleryConfiguration configuration, ILogger<GalleryController> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    [HttpGet("/galleries")]
    public IEnumerable<GalleryInfo> GetGalleries() =>
        Directory
            .GetDirectories(configuration.RootDirectory)
            .Select(_ => new GalleryInfo
            {
                GalleryId = Path.GetFileName(_),
                Title = ""
            });

    [HttpGet("/galleries/{galleryId}")]
    public IEnumerable<ImageRef> Get(string galleryId) =>
        Directory
            .GetFiles(Path.Combine(configuration.RootDirectory, galleryId), "*.jpg")
            .Where(IsNotThumbnail)
            .Select(_ =>
            {
                var imageInfo = Image.Identify(_);
                logger.LogDebug($"Image: {_}, height: {imageInfo.Height}, width: {imageInfo.Width}");
                var originalFileName = Path.GetFileName(_);
                var thumbnailFileName = $"{Path.GetFileNameWithoutExtension(_)}_thumb{Path.GetExtension(_)}";
                return new ImageRef
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

public class ImageRef
{
    public string? Thumbnail { get; init; }
    public string? Src { get; init; }
    public int? Width { get; init; }
    public int? Height { get; init; }
}

public class GalleryInfo
{
    public string? GalleryId { get; init; }
    public string? Title { get; init; }
}
