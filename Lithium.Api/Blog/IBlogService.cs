namespace Lithium.Api.Blog;

public interface IBlogService
{
    void AddPost(NewBlogPostDto post);
    void ChangePost(ChangedBlogPostDto post);
}
