namespace Lithium.Api.Blog;

public class ChangedBlogPostDto
{
    public Guid PostId { get; set; }
    public string Shortcut { get; set; }
    public string Content { get; set; }
}
