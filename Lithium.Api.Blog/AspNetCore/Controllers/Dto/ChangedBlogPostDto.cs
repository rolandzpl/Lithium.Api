namespace Lithium.Api.Blog.AspNetCore.Controllers.Dto;

public class ChangedBlogPostDto
{
    public Guid PostId { get; set; }
    public string Shortcut { get; set; }
    public string Content { get; set; }
}
