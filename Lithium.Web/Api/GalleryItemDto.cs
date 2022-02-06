namespace Lithium.Api;

public class GalleryItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
}

public class ImageItemDto
{
    public Guid ImageId { get; init; }
    public string ImageUrl { get; init; }
}
