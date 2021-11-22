using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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

public class BlogContextFactory : IDesignTimeDbContextFactory<BlogContext>
{
    public BlogContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");
        return new BlogContext(optionsBuilder.Options);
    }
}