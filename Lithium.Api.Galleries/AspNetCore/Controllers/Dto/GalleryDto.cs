namespace Lithium.Api.Controllers.Dto;

public class GalleryDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; }
}

public class GalleryWithImagesDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; }
    
    public IList<ImageDto> Images { get; init; }
}
