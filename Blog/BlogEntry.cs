using LinqSpecs;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Blog;

public class BlogContext : DbContext
{
    private readonly string dbPath;

    public DbSet<BlogPost> Posts { get; }

    public BlogContext(string dbPath)
    {
        this.dbPath = dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={dbPath}");
}

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

public enum BlogPostState
{
    Default = 0,
    Published = 1,
}

public interface IBlogPostRepository
{
    IEnumerable<BlogPost> GetBlogPosts(Specification<BlogPost> filter);
}

public class BlogPostRepository : IBlogPostRepository
{
    private readonly DbSet<BlogPost> posts;

    public BlogPostRepository(DbSet<BlogPost> posts)
    {
        this.posts = posts;
    }

    public IEnumerable<BlogPost> GetBlogPosts(Specification<BlogPost> filter)
    {
        return posts.Where(filter).ToList();
    }
}

class BlogPostFilters
{
    public static Specification<BlogPost> FilterDefault() =>
        new AdHocSpecification<BlogPost>(_ => _.State == BlogPostState.Published);

    public static Specification<BlogPost> FilterDefault(string blogId) =>
        new AdHocSpecification<BlogPost>(_ =>
            _.State == BlogPostState.Published &&
            _.BlogId == blogId);

    public static Specification<BlogPost> FilterAll() =>
        new AdHocSpecification<BlogPost>(_ => true);

    public static Specification<BlogPost> FilterAll(string blogId) =>
        new AdHocSpecification<BlogPost>(_ => _.BlogId == blogId);

    public static Specification<BlogPost> FilterByPostId(string postId) =>
        new AdHocSpecification<BlogPost>(_ => _.PostId == postId);
}


public interface IBlogService
{
    void AddPost(NewBlogPostDto post);
    void ChangePost(ChangedBlogPostDto post);
}

public class BlogService : IBlogService
{
    private readonly BlogContext db;

    public BlogService(BlogContext db)
    {
        this.db = db;
    }

    public void AddPost(NewBlogPostDto post)
    {
        db.Posts.Add(new BlogPost
        {
            PostId = Guid.NewGuid(),
            Title=post.Title,
            CreatedBy=post.CreatedBy,
            
        });
    }

    public void ChangePost(ChangedBlogPostDto post)
    {
        throw new NotImplementedException();
    }
}

public class ChangedBlogPostDto
{
}

public class NewBlogPostDto
{
}
