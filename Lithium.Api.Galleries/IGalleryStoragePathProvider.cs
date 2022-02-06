namespace Lithium.Api.Galleries;

public interface IGalleryStoragePathProvider
{
    string GetPath(Guid galeryId);
}
