namespace Lithium.Api.Galleries;

public class Gallery
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public IList<GalleryImage> Images { get; } = new List<GalleryImage>();
}
