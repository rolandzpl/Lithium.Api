using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Galleries.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGalleriesSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<GalleryConfiguration>(sp =>
            configuration.GetSection("Accounts").Get<GalleryConfiguration>());
        services.AddDbContext<GalleryContext>((sp, opt) =>
        {
            var cfg = sp.GetRequiredService<GalleryConfiguration>();
            opt.UseSqlite($"Data Source={cfg.DatabasePath}");
        });
        services.AddScoped<IGalleryRepository, GalleryRepository>();
        services.AddScoped<IGallerySevice, GallerySevice>();
        return services;
    }
}