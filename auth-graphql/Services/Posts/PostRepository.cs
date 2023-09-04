using auth_graphql.Data;
using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Services.Posts;

public class PostRepository : IPostRepository
{
    private readonly IDbContextFactory<MSSqlDbContext> _contextFactory;

    public PostRepository(IDbContextFactory<MSSqlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<IEnumerable<Post>> GetAll()
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Posts.ToListAsync();
        }
    }

    public async Task<Post> GetById(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }
    }

    public async Task<IEnumerable<Post>> GetManyByIds(IReadOnlyList<int> postIds)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Posts
                .Where(u => postIds.Contains(u.Id))
                .ToListAsync();
        }
    }
    public async Task<Post> Create(Post post)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            return post;
        }
    }

    public async Task<Post> Update(Post post)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            context.Posts.Update(post);
            await context.SaveChangesAsync();

            return post;
        }
    }

    public async Task<bool> Delete(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            var post = await context.Posts.FindAsync(id);
            if(post is null)
            {
                throw new GraphQLException(new Error("Post not found", "POST_NOT_FOUND"));
            }
            context.Posts.Remove(post);

            return await context.SaveChangesAsync() > 0;
        }
    }
}