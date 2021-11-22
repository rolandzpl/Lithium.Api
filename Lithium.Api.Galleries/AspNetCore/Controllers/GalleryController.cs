using Lithium.Api.AspNetCore;
using Lithium.Api.Controllers.Dto;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lithium.Api.Galleries.Controllers;

[Authorize("EditorAccess")]
[Authorize("AdminAccess")]
[ApiController]
[Route("[controller]")]
public class GalleryController : ControllerBase
{
    private readonly IGallerySevice galleryService;
    private readonly IGalleryRepository repository;

    public GalleryController(IGallerySevice galleryService, IGalleryRepository repository)
    {
        this.galleryService = galleryService;
        this.repository = repository;
    }

    [HttpGet("/galleries")]
    public async Task<IEnumerable<GalleryDto>> GetGalleries() =>
        await repository.GetGalleriesAsync<GalleryDto>(GalleryFilters.FilterDefault());

    [HttpPost("/galleries")]
    public void CreateGallery(NewGalleryDto gallery) =>
        galleryService.CreateGallery(gallery.Adapt<NewGalleryDto>());

    [HttpGet("/galleries/{galleryId}")]
    public async Task<ImageDto> GetImages(Guid galleryId) =>
        (await repository.GetGalleryByIdAsync<ImageDto>(galleryId))
        ?? throw new HttpResponseException(StatusCodes.Status404NotFound, galleryId);
}
