using auth_graphql.Data;
using auth_graphql.Services.Posts;

namespace auth_graphql.Schema.Queries.Posts;

[ExtendObjectType(typeof(Query))]
public class PostQuery
{
    private readonly IPostRepository _postRepository;

    public PostQuery(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    [UseDbContext(typeof(MSSqlDbContext))]
    // [UsePaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    // [UseFiltering]
    [UseSorting]
    public IQueryable<PostType> GetPosts([ScopedService] MSSqlDbContext context)
    {
        return context.Posts.Select(p => new PostType()
        {
            Id = p.Id,
            CreatorId = p.CreatorId,
            Title = p.Title,
            Content = p.Content,
            Status = p.Status,
            Access = p.Access,
            CreatedDate = p.CreatedDate,
            UpdatedDate = p.UpdatedDate
        });
    }

    public async Task<PostType> GetPostById(int id)
    {
        var post = await _postRepository.GetById(id);

        return new PostType()
        {
            Id = post.Id,
            CreatorId = post.CreatorId,
            Title = post.Title,
            Content = post.Content,
            Status = post.Status,
            Access = post.Access,
            CreatedDate = post.CreatedDate,
            UpdatedDate = post.UpdatedDate
        };
    }
}