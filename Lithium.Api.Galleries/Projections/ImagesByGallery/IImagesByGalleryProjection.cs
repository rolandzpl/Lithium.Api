namespace Lithium.Api.Galleries.Projections.ImagesByGallery;

public interface IImagesByGalleryProjection
{
    Task<IEnumerable<ImageDto>> GetImagesByGalleryAsync(Guid galleryId);
}

public class ImageDto
{
    public Guid Id { get; init; }
}
