namespace Lithium.Api.Blog;

public class BlogPost
{
    public string BlogId { get; set; }
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public string Shortcut { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedBy { get; set; }
    public BlogPostState State { get; set; }
}
