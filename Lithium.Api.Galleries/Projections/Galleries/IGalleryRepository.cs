using LinqSpecs;

namespace Lithium.Api.Galleries.Projections.Galleries;

public interface IGalleriesProjection
{
    Task<IEnumerable<GalleryDto>> GetGalleriesAsync(Specification<Gallery> filter);
}

public class GalleryDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public IList<ImageDto> Images { get; init; }
}

public class ImageDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
}
