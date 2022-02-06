using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Galleries.Projections.Images;

public class ImagesProjection : IImagesProjection
{
    private readonly GalleryContext db;

    public ImagesProjection(GalleryContext db) => this.db = db;

    public DbSet<GalleryImage> Images => db.Set<GalleryImage>();

    public async Task<ImageDto?> GetImageAsync(Guid imageId) =>
        await Images
            .Where(_ => _.Id == imageId)
            .Select(_ => new ImageDto
            {
                Id = _.Id,
                PhysicalPath = _.PhysicalPath,
                ImageType = _.ImageType.ToString()
            })
            .FirstOrDefaultAsync();
}
