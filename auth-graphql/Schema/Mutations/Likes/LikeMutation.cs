using auth_graphql.Middlewares.UseUser;
using auth_graphql.Models;
using auth_graphql.Services.Comments;
using auth_graphql.Services.Likes;
using auth_graphql.Services.Posts;
using HotChocolate.Authorization;

namespace auth_graphql.Schema.Mutations.Likes;

[ExtendObjectType(typeof(Mutation))]
public class LikeMutation
{
    private readonly ILikeRepository _likeRepository;
    public LikeMutation(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    [Authorize]
    [UseUser]
    public async Task<Like> CreateLike(
        int likeId,
        Like_Type like_Type,
        [User] User user
    )
    {
        var userId = user.Id;

        var checkLikePost = await _likeRepository.CheckUserLikePost(userId, like_Type, likeId);
        var checkLikeComment = await _likeRepository.CheckUserLikeComment(userId, like_Type, likeId);

        if (like_Type == Like_Type.Post)
        {
            if (checkLikePost != null)
            {
                throw new GraphQLException(new Error("Post was liked", "Post was liked"));
            }
        }
        else if (like_Type == Like_Type.Comment)
        {
            if (checkLikeComment != null)
            {
                throw new GraphQLException(new Error("Comment was liked", "Comment was liked"));
            }
        }
        var like = new Like()
        {
            CreatorId = userId,
            DateLiked = DateTimeOffset.UtcNow,
            Like_Id = likeId,
            Like_Type = like_Type,
        };

        await _likeRepository.Create(like);
        return like;
    }

    [Authorize]
    [UseUser]
    public async Task<bool> DeleteLike([User] User user, int id)
    {
        var userId = user.Id;
        var likeDelete = await _likeRepository.GetById(id);

        if (likeDelete is null)
        {
            throw new GraphQLException(new Error("Comment not found", "COMMENT_NOT_FOUND"));
        }
        if (likeDelete.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission", "DONT_HAVE_PERMISSION"));
        }
        return await _likeRepository.Delete(id);
    }
}