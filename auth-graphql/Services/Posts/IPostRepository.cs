using auth_graphql.Models;

namespace auth_graphql.Services.Posts;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAll();
    Task<Post> GetById(int id);
    Task<IEnumerable<Post>> GetManyByIds(IReadOnlyList<int> postIds);
    Task<Post> Create(Post post);
    Task<Post> Update(Post post);
    Task<bool> Delete(int id);
}