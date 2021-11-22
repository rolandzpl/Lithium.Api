using LinqSpecs;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Galleries;

public class GalleryRepository : IGalleryRepository
{
    private readonly GalleryContext db;

    public GalleryRepository(GalleryContext db)
    {
        this.db = db;
    }

    public IEnumerable<Gallery> GetGalleries(Specification<Gallery> filter)
    {
        var set = db.Set<Gallery>();
        return set.Where(filter).ToList();
    }

    public Gallery? GetGalleryById(Guid galleryId)
    {
        var set = db.Set<Gallery>();
        return set
            .Include(_ => _.Images)
            .Where(_ => _.Id == galleryId)
            .SingleOrDefault();
    }
}
