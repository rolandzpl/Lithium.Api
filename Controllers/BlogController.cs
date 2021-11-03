using Lithium.Api.Blog;
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
        blogService.AddPost(post);
    }

    [HttpPut("/blog/{blogId}/posts")]
    public void ChangeBlogPost(string blogId, ChangedBlogPostDto post)
    {
        blogService.ChangePost(post);
    }

    [HttpGet("/posts/{postId}")]
    public BlogPostFullDto? GetBlogPost(string postId) =>
        repository
            .GetBlogPosts(FilterByPostId(postId))
            .Select(_ => new BlogPostFullDto
            {
                PostId = _.PostId,
                Title = _.Title,
                Shortcut = _.Shortcut,
                DateCreated = _.DateCreated,
                CreatedBy = _.CreatedBy
            })
            .SingleOrDefault();

    [HttpGet("/blog/{blogId}/posts/all")]
    public IEnumerable<BlogPostDto> GetAllBlogPosts(string blogId) =>
        repository
            .GetBlogPosts(FilterAll(blogId))
            .Select(_ => new BlogPostDto
            {
                PostId = _.PostId,
                Title = _.Title,
                Shortcut = _.Shortcut,
                DateCreated = _.DateCreated,
                CreatedBy = _.CreatedBy
            })
            .ToList();

    [HttpGet("/blog/{blogId}/posts")]
    public IEnumerable<BlogPostDto> GetBlogPosts(string blogId) =>
        repository
            .GetBlogPosts(FilterDefault(blogId))
            .Select(_ => new BlogPostDto
            {
                PostId = _.PostId,
                Title = _.Title,
                Shortcut = _.Shortcut,
                DateCreated = _.DateCreated,
                CreatedBy = _.CreatedBy
            })
            .ToList();
}

public class BlogPostDto
{
    public string PostId { get; init; }
    public string Title { get; init; }
    public string Shortcut { get; init; }
    public DateTime DateCreated { get; init; }
    public string CreatedBy { get; init; }
}

public class BlogPostFullDto : BlogPostDto
{
    public string Content { get; init; }
}