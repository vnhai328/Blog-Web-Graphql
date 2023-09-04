using auth_graphql.Middlewares.UseUser;
using auth_graphql.Models;
using auth_graphql.Services.PostImages;
using auth_graphql.Services.Posts;
using auth_graphql.Services.UploadFile;
using HotChocolate.Authorization;

namespace auth_graphql.Schema.Mutations.PostImages;

[ExtendObjectType(typeof(Mutation))]
public class ImageMutation
{
    private readonly IPostImageRepository _postImageRepository;
    private readonly IPostRepository _postRepository;
    private readonly AzureUploadExtensions _uploadExtensions;

    public ImageMutation(IPostImageRepository postImageRepository, IPostRepository postRepository, AzureUploadExtensions uploadExtensions)
    {
        _postImageRepository = postImageRepository;
        _postRepository = postRepository;
        _uploadExtensions = uploadExtensions;
    }

    [Authorize]
    [UseUser]
    public async Task<Image> UploadPostImage(
        [User] User user,
        int postId,
        string localFilePath
    )
    {
        var userId = user.Id;
        var userName = user.Email;
        var checkPostUser = await _postRepository.GetById(postId);
        if(checkPostUser is null)
        {
            throw new GraphQLException(new Error("Post not found.", "POST NOT FOUND"));
        }
        if(checkPostUser.CreatorId != userId)
        {
             throw new GraphQLException(new Error("Some thing went wrong", "SOME_THING_WENT_WRONG"));
        }
        try
        {
            var imgUrl = await _uploadExtensions.UploadFile(localFilePath, userName);

            var imageUpload = new Image()
            {
                ImgUrl = imgUrl,
                PostId = postId
            };

            await _postImageRepository.Create(imageUpload);
            return imageUpload;
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }

    [Authorize]
    [UseUser]
    public async Task<bool> DeletePostImage(
        [User] User user,
        int id
    )
    {
        var userId = user.Id;
        var imageDelete = await _postImageRepository.GetById(id);
        var checkPostUser = await _postRepository.GetById(imageDelete.PostId);
        if(imageDelete is null)
        {
            throw new GraphQLException(new Error("image not found.", "IMAGE_NOT_FOUND"));
        }
        if(checkPostUser.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission to update this post.", "INVALID_PERMISSION"));
        }

        await _uploadExtensions.DeleteBlob(imageDelete.ImgUrl);
        return await _postImageRepository.Delete(id);
    }
}