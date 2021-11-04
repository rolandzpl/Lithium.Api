using LinqSpecs;
using Lithium.Api.Blog;

namespace Lithium.Api.Gallery;

public interface IGalleryRepository : IRepository<Gallery>
{
    IEnumerable<Gallery> GetGalleries(Specification<Gallery> filter);
    Gallery? GetGalleryById(Guid galleryId);
}
