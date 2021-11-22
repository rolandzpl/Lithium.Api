using LinqSpecs;

namespace Lithium.Api.Galleries;

public interface IGalleryRepository
{
    IEnumerable<Gallery> GetGalleries(Specification<Gallery> filter);
    Gallery? GetGalleryById(Guid galleryId);
}
