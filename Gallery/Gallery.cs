namespace Lithium.Api.Gallery;

public class Gallery
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public IEnumerable<GalleryImage> Images { get; } = new List<GalleryImage>();
}
