using auth_graphql.Models;

namespace auth_graphql.Services.Comments;

public interface ICommentRepository
{
    Task<Comment> GetById(int id);
    Task<IEnumerable<Comment>> GetCommentsByPostId(IReadOnlyList<int> postId);
    Task<Comment> Create(Comment comment);
    Task<Comment> Update(Comment comment);
    Task<bool> Delete(int id);
}