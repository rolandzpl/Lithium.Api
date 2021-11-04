using LinqSpecs;
using Lithium.Api.Blog;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Gallery;

public class GalleryRepository : Repository<Gallery>, IGalleryRepository
{
    public GalleryRepository(GalleryContext db) : base(db) { }

    public IEnumerable<Gallery> GetGalleries(Specification<Gallery> filter)
    {
        return base.GetItems(filter);
    }

    public Gallery? GetGalleryById(Guid galleryId)
    {
        return items
            .Include(_ => _.Images)
            .Where(_ => _.Id == galleryId)
            .SingleOrDefault();
    }
}
