using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Blog.AspNetCore;

public static class BlogCollectionExtensions
{
    public static IServiceCollection AddBlogSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<BlogConfiguration>(sp =>
            configuration.GetSection("Blog").Get<BlogConfiguration>());
        services.AddDbContext<BlogContext>((sp, opt) =>
        {
            var cfg = sp.GetRequiredService<BlogConfiguration>();
            opt.UseSqlite($"Data Source={cfg.DatabasePath}");
        });
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        services.AddScoped<IBlogService, BlogService>();
        return services;
    }
}