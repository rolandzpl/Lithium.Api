using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Gallery;

public class GalleryContext : DbContext
{
    public GalleryContext(DbContextOptions<GalleryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Gallery>();
        modelBuilder.Entity<GalleryImage>();
    }
}
