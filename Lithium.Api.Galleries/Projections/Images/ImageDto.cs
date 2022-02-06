namespace Lithium.Api.Galleries.Projections.Images;

public class ImageDto
{
    public Guid Id { get; init; }
    public string PhysicalPath { get; init; }
    public string ImageType { get; init; }
}
