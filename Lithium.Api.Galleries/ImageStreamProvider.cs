using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Lithium.Api.Galleries;

class ImageStreamProvider : IImageStreamProvider
{
    public async Task<Stream> GetImageStream(string path, int? width)
    {
        if (width.HasValue)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            var directory = Path.GetDirectoryName(path);
            var requestedFilePath = Path.ChangeExtension(
                 Path.Combine(directory, $"{fileName}__w{width}"),
                 extension);
            if (!System.IO.File.Exists(requestedFilePath))
            {
                var img = await Image.LoadAsync(path);
                img.Mutate(_ => _.Resize(Size.Truncate(new SizeF { Width = (int)width })));
                await img.SaveAsJpegAsync(requestedFilePath);
            }
            path = requestedFilePath;
        }
        return System.IO.File.OpenRead(path);
    }
}