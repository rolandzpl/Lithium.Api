using LinqSpecs;

namespace Lithium.Api.Blog;

public interface IBlogPostRepository
{
    IEnumerable<BlogPost> GetBlogPosts(Specification<BlogPost> filter);
}
