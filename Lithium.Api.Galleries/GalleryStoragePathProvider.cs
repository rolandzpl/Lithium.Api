namespace Lithium.Api.Galleries;

public class GalleryStoragePathProvider : IGalleryStoragePathProvider
{
    private readonly string storagePath;

    public GalleryStoragePathProvider(string storagePath)
    {
        this.storagePath = storagePath;
    }

    public string GetPath(Guid galeryId)
    {
        return storagePath;
    }
}
