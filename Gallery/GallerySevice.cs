namespace Lithium.Api.Gallery;

public class GallerySevice : IGallerySevice
{
    private readonly GalleryContext db;

    public GallerySevice(GalleryContext db)
    {
        this.db = db;
    }

    public void CreateGallery(NewGalleryDto gallery)
    {
        db.Set<Gallery>().Add(new Gallery()
        {
            Id = Guid.NewGuid(),
            Title = gallery.Title
        });
        db.SaveChanges();
    }
}
