using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Lithium.Api.Galleries.Projections.Galleries;
using Lithium.Api.Galleries.Projections.ImagesByGallery;
using Lithium.Api.Galleries.Projections.Images;

namespace Lithium.Api.Galleries.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGalleriesSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<GalleryConfiguration>(sp =>
            configuration.GetSection("Accounts").Get<GalleryConfiguration>());
        services.AddSingleton<IGalleryStoragePathProvider>(sp =>
        {
            var cfg = sp.GetRequiredService<GalleryConfiguration>();
            return new GalleryStoragePathProvider(cfg.RootDirectory);
        });
        services.AddDbContext<GalleryContext>((sp, opt) =>
        {
            var cfg = sp.GetRequiredService<GalleryConfiguration>();
            opt.UseSqlite($"Data Source={cfg.DatabasePath}");
        });
        services.AddScoped<IGalleriesProjection, GalleriesProjection>();
        services.AddScoped<IImagesByGalleryProjection, ImagesByGalleryProjection>();
        services.AddScoped<IImagesProjection, ImagesProjection>();
        services.AddScoped<IGallerySevice, GallerySevice>();
        services.AddSingleton<IImageStreamProvider, ImageStreamProvider>();
        return services;
    }
}
