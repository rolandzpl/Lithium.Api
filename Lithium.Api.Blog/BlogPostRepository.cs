using LinqSpecs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Blog;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BlogContext db;

    public BlogPostRepository(BlogContext db)
    {
        this.db = db;
    }

    public DbSet<BlogPost> Posts => db.Set<BlogPost>();

    public async Task<DTO?> GetBlogPostsByIdAsync<DTO>(Guid postId) =>
        await Posts
            .Where(_ => _.PostId == postId)
            .ProjectToType<DTO>()
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<DTO>> GetBlogPostsAsync<DTO>(Specification<BlogPost> filter)
    {
        return await Posts
            .Where(filter)
            .ProjectToType<DTO>()
            .ToListAsync();
    }
}
