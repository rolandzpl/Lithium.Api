namespace Lithium.Api.Galleries.Projections.Images;

public interface IImagesProjection
{
    Task<ImageDto?> GetImageAsync(Guid imageId);
}
