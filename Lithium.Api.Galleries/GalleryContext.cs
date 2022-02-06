using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lithium.Api.Galleries;

public class GalleryContext : DbContext
{
    public GalleryContext(DbContextOptions<GalleryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Gallery>().HasKey(_ => _.Id);
        modelBuilder.Entity<Gallery>().Property(_ => _.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Gallery>().HasMany(_ => _.Images).WithOne();
        modelBuilder.Entity<GalleryImage>().HasKey(_ => _.Id);
        modelBuilder.Entity<GalleryImage>().Property(_ => _.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<GalleryImage>().Property(_ => _.ImageType).HasConversion<string>().HasDefaultValue(ImageType.Jpeg);
        modelBuilder.Entity<GalleryImage>().Property(_ => _.Desription).IsRequired(false);
        modelBuilder.Entity<GalleryImage>().OwnsOne(_ => _.Resolution).Property(_ => _.Width).HasColumnName("Width");
        modelBuilder.Entity<GalleryImage>().OwnsOne(_ => _.Resolution).Property(_ => _.Height).HasColumnName("Height");
        modelBuilder.Owned<ImageResolution>();
    }
}

public class GalleryContextFactory : IDesignTimeDbContextFactory<GalleryContext>
{
    public GalleryContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GalleryContext>();
        optionsBuilder.UseSqlite("Data Source=gallery.db");
        return new GalleryContext(optionsBuilder.Options);
    }
}