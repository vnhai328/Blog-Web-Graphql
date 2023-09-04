using auth_graphql.Middlewares.UseUser;
using auth_graphql.Models;
using auth_graphql.Services.Comments;
using auth_graphql.Services.Posts;
using HotChocolate.Authorization;

namespace auth_graphql.Schema.Mutations.Comments;

[ExtendObjectType(typeof(Mutation))]
public class CommentMutation
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;

    public CommentMutation(ICommentRepository commentRepository, IPostRepository postRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }

    [Authorize]
    [UseUser]
    public async Task<CommentPayLoad> CreateComment(
        int postId,
        CommentInput commentInput,
        [User] User user
    )
    {
        var userId = user.Id;
        var checkPost = _postRepository.GetById(postId);
        if (checkPost == null)
        {
            throw new GraphQLException(new Error("Post not found", "POST_NOT_FOUND"));
        }
        var comment = new Comment()
        {
            CreatorId = userId,
            CommentText = commentInput.Comment,
            DateCommented = DateTimeOffset.UtcNow,
            PostId = postId,
        };

        await _commentRepository.Create(comment);

        return new CommentPayLoad(comment);
    }

    [Authorize]
    [UseUser]
    public async Task<CommentPayLoad> UpdateComment(
        int id,
        CommentInput commentInput,
        [User] User user
    )
    {
        var userId = user.Id;
        var commentUpdate = await _commentRepository.GetById(id);

        if (commentUpdate is null)
        {
            throw new GraphQLException(new Error("Comment not found", "COMMENT_NOT_FOUND"));
        }
        if (commentUpdate.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission", "DONT_HAVE_PERMISSION"));
        }
        commentUpdate.CommentText = commentInput.Comment;
        await _commentRepository.Update(commentUpdate);
        return new CommentPayLoad(commentUpdate);
    }

    [Authorize]
    [UseUser]
    public async Task<bool> DeleteComment([User] User user, int id)
    {
        var userId = user.Id;

        var commentDelete = await _commentRepository.GetById(id);
        if (commentDelete is null)
        {
            throw new GraphQLException(new Error("Comment not found", "COMMENT_NOT_FOUND"));
        }
        if (commentDelete.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission", "DONT_HAVE_PERMISSION"));
        }

        return await _commentRepository.Delete(id);
    }
}