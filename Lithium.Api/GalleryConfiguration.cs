namespace Lithium.Api;

public class GalleryConfiguration
{
    public string BaseUrl { get; init; }
    public string RootDirectory { get; init; }
    public string? CacheDirectory { get; init; }
    public string DatabasePath { get; init; }
}

public class BlogConfiguration
{
    public string DatabasePath { get; init; }
}
