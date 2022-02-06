using Refit;

namespace Lithium.Api;

public interface IBlogApi
{
    [Get("/blog/{blogId}/posts")]
    Task<IEnumerable<BlogArticleDto>> GetArticles(string blogId);
}
