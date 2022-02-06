using Lithium.Api.Galleries.Projections.Images;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Lithium.Api.Galleries.AspNetCore;

public class GalleryImageFileProvider : IFileProvider
{
    private readonly IImagesProjection imagesProjection;

    public GalleryImageFileProvider(IImagesProjection imagesProjection) =>
        this.imagesProjection = imagesProjection;

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        throw new NotImplementedException();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        var image = imagesProjection.GetImageAsync(DecodeImageId(subpath)).Result;
        return image != null
            ? new ImageFileInfo(new FileInfo(image.PhysicalPath))
            : new NotFoundFileInfo(subpath);
    }

    protected Guid DecodeImageId(string encodedImageId) =>
        Guid.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(encodedImageId)));

    public IChangeToken Watch(string filter)
    {
        throw new NotImplementedException();
    }
}

class ImageFileInfo : IFileInfo
{
    private FileInfo fileInfo;

    public ImageFileInfo(FileInfo fileInfo)
    {
        this.fileInfo = fileInfo;
    }

    public bool Exists => fileInfo.Exists;

    public long Length => fileInfo.Length;

    public string PhysicalPath => throw new NotImplementedException();

    public string Name => fileInfo.Name;

    public DateTimeOffset LastModified => fileInfo.LastWriteTimeUtc;

    public bool IsDirectory => false;

    public Stream CreateReadStream() => fileInfo.OpenRead();
}