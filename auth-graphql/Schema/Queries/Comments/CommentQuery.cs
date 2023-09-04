using auth_graphql.Data;

namespace auth_graphql.Schema.Queries.Comments;

[ExtendObjectType(typeof(Query))]
public class CommentQuery
{
    [UseDbContext(typeof(MSSqlDbContext))]
    // [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
    [UseProjection]
    // [UseFiltering]
    [UseSorting]

    public IQueryable<CommentType> GetComments([ScopedService] MSSqlDbContext context)
    {
        return context.Comments.Select(c => new CommentType()
        {
            Id = c.Id,
            CreatorId = c.CreatorId,
            CommentText = c.CommentText,
            DateCommented = c.DateCommented,
            PostId = c.PostId
        });
    }

}