using auth_graphql.Data;
using auth_graphql.Services.Likes;

namespace auth_graphql.Schema.Queries.Likes;

[ExtendObjectType(typeof(Query))]
public class LikeQuery
{
    private readonly ILikeRepository _likeRepository;

    public LikeQuery(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }
    
    public async Task<LikeType> GetLikeById(int id)
    {
        var like = await _likeRepository.GetById(id);
        return new LikeType()
        {
            Id = like.Id,
            CreatorId = like.CreatorId,
            DateLiked = like.DateLiked,
            Like_Id = like.Like_Id,
            Like_Type = like.Like_Type
        };
    }
}