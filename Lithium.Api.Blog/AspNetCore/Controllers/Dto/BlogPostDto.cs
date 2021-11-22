namespace Lithium.Api.Blog.AspNetCore.Controllers.Dto;

public class BlogPostDto
{
    public Guid PostId { get; init; }
    public string Title { get; init; }
    public string Shortcut { get; init; }
    public DateTime DateCreated { get; init; }
    public string CreatedBy { get; init; }
}
