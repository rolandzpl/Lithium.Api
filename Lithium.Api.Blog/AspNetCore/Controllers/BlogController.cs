using Lithium.Api.AspNetCore;
using Lithium.Api.Blog.AspNetCore.Controllers.Dto;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Lithium.Api.Blog.BlogPostFilters;

namespace Lithium.Api.Blog.AspNetCore.Controllers;

[Authorize("EditorAccess")]
[Authorize("AdminAccess")]
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
    public void CreateNewBlogPost(string blogId, NewBlogPostDto post) =>
        blogService.AddPost(post.Adapt<NewBlogPostDto>(GetTypeAdapterConfig(blogId)));

    private TypeAdapterConfig GetTypeAdapterConfig(string blogId) =>
        TypeAdapterConfig<NewBlogPostDto, Blog.NewBlogPostDto>
            .NewConfig()
            .Map(_ => _.BlogId, _ => blogId)
            .Map(_ => _.CreatedBy, _ => User.Identity.Name)
            .Config;

    [HttpPut("/blog/{blogId}/posts")]
    public void ChangeBlogPost(string blogId, ChangedBlogPostDto post) =>
        blogService.ChangePost(post.Adapt<Blog.ChangedBlogPostDto>());

    [AllowAnonymous]
    [HttpGet("/posts/{postId}")]
    public async Task<BlogPostFullDto> GetBlogPost(Guid postId) =>
        (await repository.GetBlogPostsByIdAsync<BlogPostFullDto>(postId))
        ?? throw new HttpResponseException(StatusCodes.Status404NotFound, postId);

    [HttpGet("/blog/{blogId}/posts/all")]
    public async Task<IEnumerable<BlogPostDto>> GetAllBlogPosts(string blogId) =>
        await repository.GetBlogPostsAsync<BlogPostDto>(FilterAll(blogId));

    [AllowAnonymous]
    [HttpGet("/blog/{blogId}/posts")]
    public async Task<IEnumerable<BlogPostDto>> GetBlogPosts(string blogId) =>
        await repository.GetBlogPostsAsync<BlogPostDto>(FilterDefault(blogId));
}
