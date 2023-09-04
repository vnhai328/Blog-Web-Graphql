using auth_graphql.Data;
using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Services.Comments;

public class CommentRepository : ICommentRepository
{
    private readonly IDbContextFactory<MSSqlDbContext> _contextFactory;
    public CommentRepository(IDbContextFactory<MSSqlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<Comment> GetById(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostId(IReadOnlyList<int> postIds)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Comments.Where(c => postIds.Contains(c.PostId)).ToListAsync();
        }
    }
    public async Task<Comment> Create(Comment comment)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();

            return comment;
        }
    }
    public async Task<Comment> Update(Comment comment)
    {
        using(MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            context.Comments.Update(comment);
            await context.SaveChangesAsync();

            return comment;
        }
    }
    public async Task<bool> Delete(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            var comment = await context.Comments.FindAsync(id);
            if(comment is null)
            {
                throw new GraphQLException(new Error("Comment not found", "COMMENT_NOT_FOUND"));
            }
            context.Comments.Remove(comment);

            return await context.SaveChangesAsync() > 0;
        }
    }
}