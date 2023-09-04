using auth_graphql.Models;

namespace auth_graphql.Services.PostImages;

public interface IPostImageRepository
{
    Task<Image> GetById(int id);
    Task<IEnumerable<Image>> GetImagesByPostIds(IReadOnlyList<int> postIds);
    Task<Image> Create(Image image);
    Task<bool> Delete(int id);
}