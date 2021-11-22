using LinqSpecs;

namespace Lithium.Api.Blog;

public interface IBlogPostRepository
{
    Task<DTO?> GetBlogPostsByIdAsync<DTO>(Guid postId);

    Task<IEnumerable<DTO>> GetBlogPostsAsync<DTO>(Specification<BlogPost> filter);
}
