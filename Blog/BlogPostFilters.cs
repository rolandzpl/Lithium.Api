using LinqSpecs;

namespace Lithium.Api.Blog;

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

    public static Specification<BlogPost> FilterByPostId(Guid postId) =>
        new AdHocSpecification<BlogPost>(_ => _.PostId == postId);
}
