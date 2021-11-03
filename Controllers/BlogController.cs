using Lithium.Api.Blog;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using static Lithium.Api.Blog.BlogPostFilters;

namespace Lithium.Api.Gallery.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogPostRepository repository;
    private readonly IBlogService blogService;

    public BlogController(IBlogPostRepository repository, IBlogService blogService)
    {
        this.repository = repository;
        this.blogService = blogService;
    }

    [HttpPost("/blog/{blogId}/posts")]
    public void CreateNewBlogPost(string blogId, NewBlogPostDto post)
    {
        var cfg = TypeAdapterConfig<NewBlogPostDto, Blog.NewBlogPostDto>
            .NewConfig()
            .Map(_ => _.BlogId, _ => blogId);
        blogService.AddPost(post.Adapt<Blog.NewBlogPostDto>(cfg.Config));
    }

    [HttpPut("/blog/{blogId}/posts")]
    public void ChangeBlogPost(string blogId, ChangedBlogPostDto post)
    {
        blogService.ChangePost(post.Adapt<Blog.ChangedBlogPostDto>());
    }

    [HttpGet("/posts/{postId}")]
    public BlogPostFullDto? GetBlogPost(Guid postId) =>
        repository
            .GetBlogPosts(FilterByPostId(postId))
            .Select(_ => _.Adapt<BlogPostFullDto>())
            .SingleOrDefault();

    [HttpGet("/blog/{blogId}/posts/all")]
    public IEnumerable<BlogPostDto> GetAllBlogPosts(string blogId) =>
        repository
            .GetBlogPosts(FilterAll(blogId))
            .Select(_ => _.Adapt<BlogPostDto>())
            .ToList();

    [HttpGet("/blog/{blogId}/posts")]
    public IEnumerable<BlogPostDto> GetBlogPosts(string blogId) =>
        repository
            .GetBlogPosts(FilterDefault(blogId))
            .Select(_ => _.Adapt<BlogPostDto>())
            .ToList();
}
