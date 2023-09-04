using auth_graphql.Data;
using auth_graphql.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_graphql.Services.Likes;

public class LikeRepository : ILikeRepository
{
    private readonly IDbContextFactory<MSSqlDbContext> _contextFactory;

    public LikeRepository(IDbContextFactory<MSSqlDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<IEnumerable<Like>> GetLikeByPostId(IReadOnlyList<int> postIds)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Likes.Where(l => postIds.Contains(l.Like_Id) && l.Like_Type == Like_Type.Post).ToListAsync();
        }
    }
     public async Task<IEnumerable<Like>> GetLikeByCommentId(IReadOnlyList<int> commentIds)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Likes.Where(l => commentIds.Contains(l.Like_Id) && l.Like_Type == Like_Type.Comment).ToListAsync();
        }
    }
    public async Task<Like> GetById(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Likes.FirstOrDefaultAsync(l => l.Id == id);
        }
    }
    public async Task<Like> CheckUserLikePost(Guid userId, Like_Type like_Type, int likeId)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Likes.FirstOrDefaultAsync(l => l.CreatorId == userId && l.Like_Type == Like_Type.Post && l.Like_Id == likeId);
        }
    }
    public async Task<Like> CheckUserLikeComment(Guid userId, Like_Type like_Type, int likeId)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Likes.FirstOrDefaultAsync(l => l.CreatorId == userId && l.Like_Type == Like_Type.Comment && l.Like_Id == likeId);
        }
    }
    public async Task<Like> Create(Like like)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            await context.Likes.AddAsync(like);
            await context.SaveChangesAsync();

            return like;
        }
    }
    public async Task<bool> Delete(int id)
    {
        using (MSSqlDbContext context = _contextFactory.CreateDbContext())
        {
            var like = await context.Likes.FindAsync(id);
            if(like is null)
            {
                throw new GraphQLException(new Error("Some thing wrong", "SOME_THING_WRONG"));
            }
            context.Likes.Remove(like);

            return await context.SaveChangesAsync() > 0;
        }
    }
}