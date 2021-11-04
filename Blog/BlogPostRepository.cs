using LinqSpecs;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Blog;

public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(BlogContext db) : base(db) { }

    public IEnumerable<BlogPost> GetBlogPosts(Specification<BlogPost> filter)
    {
        return base.GetItems(filter);
    }
}

public interface IRepository<TEntity>
{
    IEnumerable<TEntity> GetItems(Specification<TEntity> filter);
}

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbSet<TEntity> items;

    public Repository(DbContext db)
    {
        this.items = db.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetItems(Specification<TEntity> filter)
    {
        return items.Where(filter).ToList();
    }
}
