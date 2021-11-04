using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Blog;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>(b =>
        {
            b.HasKey(_ => _.PostId);
        });
    }
}
