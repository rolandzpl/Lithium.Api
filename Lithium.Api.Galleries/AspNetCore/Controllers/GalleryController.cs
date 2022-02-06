using System.Text;
using Lithium.Api.AspNetCore;
using Lithium.Api.Controllers.Dto;
using Lithium.Api.Galleries.Projections.Galleries;
using Lithium.Api.Galleries.Projections.Images;
using Lithium.Api.Galleries.Projections.ImagesByGallery;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using GalleryDto = Lithium.Api.Controllers.Dto.GalleryDto;
using ImageDto = Lithium.Api.Controllers.Dto.ImageDto;

namespace Lithium.Api.Galleries.Controllers;

// [Authorize("EditorAccess")]
[ApiController]
[Route("[controller]")]
public class GalleryController : ControllerBase
{
    private readonly IGallerySevice galleryService;
    private readonly IGalleriesProjection galleriesProjection;
    private readonly IImagesByGalleryProjection imagesByGalleryProjection;
    private readonly IImagesProjection imagesProjection;
    private readonly IImageStreamProvider imageStreamProvider;

    public GalleryController(
        IGallerySevice galleryService,
        IGalleriesProjection galleriesProjection,
        IImagesByGalleryProjection imagesByGalleryProjection,
        IImagesProjection imagesProjection,
        IImageStreamProvider imageStreamProvider)
    {
        this.galleryService = galleryService;
        this.galleriesProjection = galleriesProjection;
        this.imagesByGalleryProjection = imagesByGalleryProjection;
        this.imagesProjection = imagesProjection;
        this.imageStreamProvider = imageStreamProvider;
    }

    [AllowAnonymous]
    [HttpGet("/gallery/list")]
    public async Task<IEnumerable<GalleryDto>> GetGalleries() =>
        (await galleriesProjection.GetGalleriesAsync(GalleryFilters.FilterDefault()))
            .Adapt<IEnumerable<GalleryDto>>();

    [AllowAnonymous]
    [HttpGet("/gallery/list/all")]
    public async Task<IEnumerable<GalleryDto>> GetAllGalleries() =>
        (await galleriesProjection.GetGalleriesAsync(GalleryFilters.FilterDefault()))
            .Adapt<IEnumerable<GalleryDto>>();

    [AllowAnonymous]
    [HttpPost("/gallery")]
    public async Task CreateGallery(NewGalleryDto gallery) =>
        await galleryService.CreateGalleryAsync(gallery.Adapt<NewGalleryDto>());

    [AllowAnonymous]
    [HttpPut("/gallery/{galleryId}")]
    public async Task AmendGallery(Guid galleryId, ChangedGalleryDto gallery) =>
        await galleryService.AmendGallery(galleryId, gallery);

    [AllowAnonymous]
    [HttpDelete("/gallery/{galleryId}")]
    public async Task DeleteGallery(Guid galleryId) =>
        await galleryService.DeleteGallery(galleryId);

    [AllowAnonymous]
    [HttpGet("/gallery/{galleryId}")]
    public async Task<GalleryWithImagesDto> GetGallery(Guid galleryId) =>
        (
            (await galleriesProjection.GetGalleriesAsync(GalleryFilters.FilterById(galleryId)))
                .FirstOrDefault()
            ?? throw new HttpResponseException(StatusCodes.Status404NotFound, galleryId)
        )
        .Adapt<GalleryWithImagesDto>();

    [AllowAnonymous]
    [HttpGet("/gallery/{galleryId}/images")]
    public async Task<IEnumerable<ImageDto>> GetImages(Guid galleryId) =>
        (
            await imagesByGalleryProjection.GetImagesByGalleryAsync(galleryId)
            ?? throw new HttpResponseException(StatusCodes.Status404NotFound, galleryId)
        )
        .Select(_ => new ImageDto
        {
            ImageId = _.Id,
            ImageUrl = $"{Request.Scheme}://{Request.Host}/gallery/images/{EncodeImageId(_.Id)}"
        });

    [AllowAnonymous]
    [HttpPost("/gallery/{galleryId}/images")]
    public async Task AddImage(Guid galleryId, string title, string description, IFormFile file) =>
        await galleryService.AddImagesAsync(galleryId, title, description, Path.GetExtension(file.FileName),
            async (string path) =>
            {
                var image = await Image.LoadAsync(file.OpenReadStream());
                await image.SaveAsJpegAsync(path);
                return image.Width;
            });

    [AllowAnonymous]
    [HttpDelete("/gallery/images/{imageId}")]
    public async Task DeleteImage(Guid imageId) =>
        await galleryService.DeleteImageAsync(imageId);

    [AllowAnonymous]
    [HttpGet("/gallery/images/{encodedImageId}")]
    public async Task<IActionResult> GetImage(string encodedImageId, int? width)
    {
        var image = await imagesProjection.GetImageAsync(DecodeImageId(encodedImageId));
        if (image == null || !System.IO.File.Exists(image.PhysicalPath))
        {
            throw new HttpResponseException(StatusCodes.Status404NotFound, encodedImageId);
        }
        return new FileStreamResult(
            await imageStreamProvider.GetImageStream(image.PhysicalPath, width),
            $"image/{image.ImageType.ToLower()}");
    }

    protected string EncodeImageId(Guid imageId) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(imageId.ToString()));

    protected Guid DecodeImageId(string encodedImageId) =>
        Guid.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(encodedImageId)));
}

