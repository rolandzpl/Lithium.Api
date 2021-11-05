using Lithium.Api.Blog;
using Lithium.Api.Gallery;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Lithium.Api;

public class IntegrationTests
{
    [Test]
    public async Task Test1()
    {
        var client = factory.CreateClient();
        var result = await client.GetAsync($"{factory.Server.BaseAddress}/galleries");
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [SetUp]
    public void Setup()
    {
        factory = new WebApplicationFactory<IntegrationTests>();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<GalleryConfiguration>(sp => new GalleryConfiguration { });
                services.AddSingleton<BlogConfiguration>(sp => new BlogConfiguration { });
                services.AddDbContext<BlogContext>((sp, opt) =>
                {
                    var cfg = sp.GetRequiredService<BlogConfiguration>();
                    opt.UseSqlite($"Data Source={cfg.DatabasePath}");
                });
                services.AddDbContext<GalleryContext>((sp, opt) =>
                {
                    var cfg = sp.GetRequiredService<GalleryConfiguration>();
                    opt.UseSqlite($"Data Source={cfg.DatabasePath}");
                });
                services.AddScoped<IBlogPostRepository, BlogPostRepository>();
                services.AddScoped<IBlogService, BlogService>();
                services.AddScoped<IGalleryRepository, GalleryRepository>();
                services.AddScoped<IGallerySevice, GallerySevice>();
                services.AddControllers(opt => opt.Filters.Add(new HttpResponseExceptionFilter()));
                services.AddEndpointsApiExplorer();
            });

    private WebApplicationFactory<IntegrationTests> factory;
}