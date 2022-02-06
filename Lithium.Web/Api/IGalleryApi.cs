using Refit;

namespace Lithium.Api;

public interface IGalleryApi
{
    [Get("/gallery/{galleryId}/images")]
    Task<IEnumerable<ImageItemDto>> GetImages(Guid galleryId);

    [Get("/gallery/list")]
    Task<IEnumerable<GalleryItemDto>> GetGalleries();
}
