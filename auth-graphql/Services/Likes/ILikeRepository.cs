using auth_graphql.Models;

namespace auth_graphql.Services.Likes;

public interface ILikeRepository
{
    Task<IEnumerable<Like>> GetLikeByPostId(IReadOnlyList<int> postIds);
    Task<IEnumerable<Like>> GetLikeByCommentId(IReadOnlyList<int> commentIds);
    Task<Like> GetById(int id);
    Task<Like> CheckUserLikePost(Guid userId, Like_Type like_Type, int likeId);
    Task<Like> CheckUserLikeComment(Guid userId, Like_Type like_Type, int likeId);
    Task<Like> Create(Like like);
    Task<bool> Delete(int id);
}