using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lithium.Api.Galleries;

public class GalleryContext : DbContext
{
    public GalleryContext(DbContextOptions<GalleryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Gallery>()
            .HasMany(_ => _.Images).WithOne();
        modelBuilder.Entity<GalleryImage>()
            .Property(_ => _.Desription).IsRequired(false);
    }
}

public class GalleryContextFactory : IDesignTimeDbContextFactory<GalleryContext>
{
    public GalleryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GalleryContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");
        return new GalleryContext(optionsBuilder.Options);
    }
}