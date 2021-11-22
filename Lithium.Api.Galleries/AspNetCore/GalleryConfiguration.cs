namespace Lithium.Api.Galleries.AspNetCore;

public class GalleryConfiguration
{
    public string BaseUrl { get; init; }
    public string RootDirectory { get; init; }
    public string? CacheDirectory { get; init; }
    public string DatabasePath { get; init; }
}
