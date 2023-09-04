using auth_graphql.Data;
using auth_graphql.Schema.Queries.People;
using auth_graphql.Schema.Queries.Posts;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Schema.Queries;

public class Query
{
    [UseDbContext(typeof(MSSqlDbContext))]
    public async Task<IEnumerable<ISearchResultType>> Search(string term, [ScopedService] MSSqlDbContext context)
    {
        var people = await context.People
            .Where(p => p.Name.Contains(term) || p.Email.Contains(term))
            .Select(p => new PersonType()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                ImageUri = p.ImageUri
            }).ToListAsync();

        var posts = await context.Posts
            .Where(p => p.Title.Contains(term) || p.Content.Contains(term))
            .Select(p => new PostType()
            {
                Id = p.Id,
                CreatorId = p.CreatorId,
                Title = p.Title,
                Content = p.Content,
                Status = p.Status,
                Access = p.Access,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate
            }).ToListAsync();

        return new List<ISearchResultType>()
            .Concat(people)
            .Concat(posts);
    }
}