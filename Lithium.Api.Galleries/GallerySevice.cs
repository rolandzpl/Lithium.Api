using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Galleries;

public class GallerySevice : IGallerySevice
{
    private static readonly string[] supportedExtensions = new[] { "jpg", "jpeg", "png", "tiff" };

    private readonly GalleryContext db;

    private readonly IGalleryStoragePathProvider pathProvider;

    protected DbSet<Gallery> Galleries => db.Set<Gallery>();

    protected DbSet<GalleryImage> Images => db.Set<GalleryImage>();

    public GallerySevice(GalleryContext db, IGalleryStoragePathProvider pathProvider)
    {
        this.db = db;
        this.pathProvider = pathProvider;
    }

    public async Task AddImagesAsync(Guid galleryId, string title, string description, string extension, Func<string, Task<int>> writeFileAsync)
    {
        if (!supportedExtensions.Contains(extension.TrimStart('.').ToLowerInvariant()))
        {
            throw new NotSupportedException($"Files with extensions {extension} are not supported");
        }
        var gallery = await Galleries.FindAsync(galleryId);
        if (gallery == null)
        {
            throw new KeyNotFoundException();
        }
        var filePath = Path.Combine(pathProvider.GetPath(galleryId), Path.GetRandomFileName());
        await writeFileAsync(filePath);
        var image = new GalleryImage()
        {
            Title = title,
            Desription = description,
            PhysicalPath = filePath
        };
        gallery.Images.Add(image);
        Images.Add(image);
        await db.SaveChangesAsync(true);
    }

    public async Task CreateGalleryAsync(NewGalleryDto gallery)
    {
        Galleries.Add(new Gallery()
        {
            Title = gallery.Title
        });
        await db.SaveChangesAsync();
    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        var image = db.Entry<GalleryImage>(new GalleryImage()
        {
            Id = imageId
        });
        image.State = EntityState.Deleted;
        await db.SaveChangesAsync();
    }

    public async Task AmendGallery(Guid galleryId, ChangedGalleryDto amendedGallery)
    {
        var gallery = await Galleries.FindAsync(galleryId);
        if (gallery == null)
        {
            throw new KeyNotFoundException();
        }
        if (amendedGallery.Title != null)
        {
            gallery.Title = amendedGallery.Title;
        }
        await db.SaveChangesAsync();
    }

    public async Task DeleteGallery(Guid galleryId)
    {
        var gallery = db.Entry<Gallery>(new Gallery()
        {
            Id = galleryId
        });
        gallery.State = EntityState.Deleted;
        await db.SaveChangesAsync();
    }
}
