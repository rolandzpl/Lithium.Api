namespace Lithium.Api.Blog;

public class BlogService : IBlogService
{
    private readonly BlogContext db;

    public BlogService(BlogContext db)
    {
        this.db = db;
    }

    public void AddPost(NewBlogPostDto post)
    {
        db.Set<BlogPost>().Add(new BlogPost
        {
            BlogId = post.BlogId,
            PostId = Guid.NewGuid(),
            Title = post.Title,
            CreatedBy = "admin",
            Content = post.Content,
            Shortcut = post.Shortcut,
            State = BlogPostState.Default,
            DateCreated = DateTime.UtcNow

        });
        db.SaveChanges();
    }

    public void ChangePost(ChangedBlogPostDto post)
    {
        var existingPost = db.Set<BlogPost>()
            .SingleOrDefault(_ => _.PostId == post.PostId);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"Post {post.PostId} does not exist");
        }
        if (post.Shortcut != null)
        {
            existingPost.Shortcut = post.Shortcut;
        }
        if (post.Content != null)
        {
            existingPost.Content = post.Content;
        }
        db.SaveChanges();
    }
}
