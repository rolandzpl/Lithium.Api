namespace Lithium.Api.Galleries;

public interface IGallerySevice
{
    Task CreateGalleryAsync(NewGalleryDto newGalleryDto);

    Task AddImagesAsync(Guid galleryId, string title, string description, string extension, Func<string, Task<int>> writeFile);

    Task DeleteImageAsync(Guid imageId);

    Task AmendGallery(Guid galleryId, ChangedGalleryDto gallery);

    Task DeleteGallery(Guid galleryId);
}

public class NewGalleryDto
{
    public string Title { get; init; }
    public string Description { get; init; }
}

public class ChangedGalleryDto
{
    public string Title { get; init; }
    public string Description { get; init; }
}