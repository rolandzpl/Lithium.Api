using LinqSpecs;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Lithium.Api.Galleries;

public class GalleryRepository : IGalleryRepository
{
    private readonly GalleryContext db;

    public GalleryRepository(GalleryContext db)
    {
        this.db = db;
    }

    public DbSet<Gallery> Galleries => db.Set<Gallery>();

    public async Task<IEnumerable<DTO>> GetGalleriesAsync<DTO>(Specification<Gallery> filter) =>
        await Galleries
            .Include(_ => _.Images)
            .Where(filter)
            .ProjectToType<DTO>()
            .ToListAsync();

    public async Task<DTO?> GetGalleryByIdAsync<DTO>(Guid galleryId) =>
        await Galleries
            .Include(_ => _.Images)
            .Where(_ => _.Id == galleryId)
            .ProjectToType<DTO>()
            .SingleOrDefaultAsync();
}
