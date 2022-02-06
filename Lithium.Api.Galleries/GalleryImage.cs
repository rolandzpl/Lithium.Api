namespace Lithium.Api.Galleries;

public class GalleryImage
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public ImageType ImageType { get; init; }
    public ImageResolution? Resolution { get; init; }
    public string Desription { get; set; }
    public string PhysicalPath { get; set; }
}

public enum ImageType
{
    Jpeg,
    Png,
    Tiff
}

public class ImageResolution
{
    public int Height { get; init; }
    public int Width { get; init; }
}
