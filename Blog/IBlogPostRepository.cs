using LinqSpecs;

namespace Lithium.Api.Blog;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    IEnumerable<BlogPost> GetBlogPosts(Specification<BlogPost> filter);
}
