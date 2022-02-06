using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Galleries.Projections.ImagesByGallery;

public class ImagesByGalleryProjection : IImagesByGalleryProjection
{
    private readonly GalleryContext db;

    public ImagesByGalleryProjection(GalleryContext db) => this.db = db;

    public DbSet<Gallery> Galleries => db.Set<Gallery>();

    public async Task<IEnumerable<ImageDto>> GetImagesByGalleryAsync(Guid galleryId) =>
        await Galleries
            .Where(_ => _.Id == galleryId)
            .Include(_ => _.Images)
            .SelectMany(_ => _.Images)
            .Select(_ => new ImageDto
            {
                Id = _.Id
            })
            .ToListAsync();
}
