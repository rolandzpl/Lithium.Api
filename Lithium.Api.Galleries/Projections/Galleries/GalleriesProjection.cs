using LinqSpecs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Galleries.Projections.Galleries;

public class GalleriesProjection : IGalleriesProjection
{
    private readonly GalleryContext db;

    public GalleriesProjection(GalleryContext db) => this.db = db;

    public DbSet<Gallery> Galleries => db.Set<Gallery>();

    public async Task<IEnumerable<GalleryDto>> GetGalleriesAsync(Specification<Gallery> filter) =>
        await Galleries
            .Include(_ => _.Images)
            .Where(filter)
            .ProjectToType<GalleryDto>()
            .ToListAsync();
}
