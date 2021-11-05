using Lithium.Api.Controllers.Dto;
using Lithium.Api.Gallery;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NewGalleryDto = Lithium.Api.Controllers.Dto.NewGalleryDto;

namespace Lithium.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GalleryController : ControllerBase
{
    private readonly IGallerySevice galleryService;
    private readonly IGalleryRepository repository;
    private readonly ILogger<GalleryController> logger;

    public GalleryController(IGallerySevice galleryService, IGalleryRepository repository, ILogger<GalleryController> logger)
    {
        this.galleryService = galleryService;
        this.repository = repository;
        this.logger = logger;
    }

    [HttpGet("/galleries")]
    public IEnumerable<GalleryDto> GetGalleries()
    {
        return repository
            .GetGalleries(GalleryFilters.FilterDefault())
            .Select(_ => _.Adapt<GalleryDto>())
            .ToList();
    }

    [HttpPost("/galleries")]
    public void CreateGallery(NewGalleryDto gallery)
    {
        galleryService.CreateGallery(gallery.Adapt<Gallery.NewGalleryDto>());
    }

    [HttpGet("/galleries/{galleryId}")]
    public IEnumerable<ImageDto> GetImages(Guid galleryId)
    {
        var gallery = repository
            .GetGalleryById(galleryId);
        if (gallery == null)
        {
            throw new HttpResponseException(StatusCodes.Status404NotFound, galleryId);
        }
        return gallery.Images
            .Select(img => img.Adapt<ImageDto>())
            .ToList();
    }
}
