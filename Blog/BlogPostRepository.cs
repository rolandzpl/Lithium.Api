using LinqSpecs;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Blog;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly DbSet<BlogPost> posts;

    public BlogPostRepository(BlogContext db)
    {
        this.posts = db.Set<BlogPost>();
    }

    public IEnumerable<BlogPost> GetBlogPosts(Specification<BlogPost> filter)
    {
        return posts.Where(filter).ToList();
    }
}
