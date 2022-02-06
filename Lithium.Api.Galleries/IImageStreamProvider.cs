namespace Lithium.Api.Galleries;

public interface IImageStreamProvider
{
    Task<Stream> GetImageStream(string path, int? width);
}
