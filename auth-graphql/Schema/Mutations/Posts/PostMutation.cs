using System.Security.Claims;
using auth_graphql.Data;
using auth_graphql.Middlewares.UseUser;
using auth_graphql.Models;
using auth_graphql.Services.Posts;
using HotChocolate.Authorization;

namespace auth_graphql.Schema.Mutations.Posts;

[ExtendObjectType(typeof(Mutation))]
public class PostMutation
{
    private readonly IPostRepository _postRepository;

    public PostMutation(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [UseUser]
    [Authorize]
    public async Task<PostPayload> CreatePost(
        PostInput input,
        CancellationToken cancellationToken,
        [User] User user
    )
    {
        var userId = user.Id;
        if(input.Content == "")
        {
            throw new GraphQLException(new Error("Content not null", "CONTENT_NOT_NULL"));
        }
        var post = new Post
        {
            CreatorId = userId,
            Title = input.Title,
            Content = input.Content,
            Status = "Active",
            Access = "Public",
            CreatedDate = DateTimeOffset.UtcNow
        };
        
        await _postRepository.Create(post);

        return new PostPayload(post);
    }

    [UseUser]
    [Authorize]
    public async Task<PostPayload> UpdatePost(int id, 
        PostInput input,
        [User] User user
    )
    {
        var userId = user.Id;
        var postUpdate = await _postRepository.GetById(id);

        if(postUpdate is null)
        {
            throw new GraphQLException(new Error("Post not found.", "POST_NOT_FOUND"));
        }
        if(postUpdate.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission to update this post.", "INVALID_PERMISSION"));
        }

        postUpdate.Id = id;
        postUpdate.Title = input.Title;
        postUpdate.Content = input.Content;
        postUpdate.UpdatedDate = DateTimeOffset.UtcNow;

        await _postRepository.Update(postUpdate);

        return new PostPayload(postUpdate);
    }

    [UseUser]
    [Authorize]
    public async Task<bool> DeletePost([User] User user, int id)
    {
        var userId = user.Id;
        var postDelete = await _postRepository.GetById(id);

        if (postDelete is null)
        {
            throw new GraphQLException(new Error("Post not found.", "POST_NOT_FOUND"));
        }

        if (postDelete.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission to update this post.", "INVALID_PERMISSION"));
        }

        return await _postRepository.Delete(id);
    }
}