using auth_graphql.DataLoaders;
using auth_graphql.Models;
using auth_graphql.Schema.Queries.Comments;
using auth_graphql.Schema.Queries.Likes;
using auth_graphql.Schema.Queries.People;
using HotChocolate.Resolvers;

namespace auth_graphql.Schema.Queries.Posts;

public class PostType : ISearchResultType
{
    [IsProjected(true)]
    public int Id { get; set; }
    [IsProjected(true)]
    public Guid CreatorId { get; set; }
    public async Task<PersonType> Creator([Service] PersonByUserIdDataLoader personByUserIdData)
    {
        var person = await personByUserIdData.LoadAsync(CreatorId, CancellationToken.None);
        return new PersonType()
        {
            Id = person.Id,
            UserId = person.UserId,
            Email = person.Email,
            Name = person.Name,
            LastSeen = person.LastSeen,
            ImageUri = person.ImageUri
        };
    }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public string Access { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public async Task<IEnumerable<Image>> PostImages([Service] PostImageDataLoader postImageDataLoader, IResolverContext context)
    {
        var imageResponse = await postImageDataLoader.LoadAsync(Id, CancellationToken.None);

        var postImages = imageResponse.Select(i => new Image{
            Id = i.Id,
            ImgUrl = i.ImgUrl,
            PostId = i.PostId
        }).ToList();

        return postImages;
    }
    public async Task<IEnumerable<CommentType>> Comments([Service] CommentDataLoader commentDataLoader, IResolverContext context)
    {
        var commentReponse = await commentDataLoader.LoadAsync(Id, CancellationToken.None);

        var comments = commentReponse.Select(c => new CommentType()
        {
           Id = c.Id,
           CommentText = c.CommentText,
           DateCommented = c.DateCommented,
           CreatorId = c.CreatorId
        }).ToList();

        return comments;
    }

    public async Task<int> CommmentCount([Service] CommentDataLoader commentDataLoader, IResolverContext context)
    {
        var commentReponse = await commentDataLoader.LoadAsync(Id, CancellationToken.None);

        return commentReponse.Count();
    }

    public async Task<IEnumerable<LikeType>> PostLikes([Service] PostLikeDataLoader likeDataLoader)
    {
        var likeResponse = await likeDataLoader.LoadAsync(Id, CancellationToken.None);

        var postLike = likeResponse.Select(l => new LikeType()
        {
            Id = l.Id,
            CreatorId = l.CreatorId,
            DateLiked = l.DateLiked,
            Like_Id = l.Like_Id,
            Like_Type = l.Like_Type
        });

        return postLike;
    }
    public async Task<int> LikeCount([Service] PostLikeDataLoader likeDataLoader, IResolverContext context)
    {
        var likeResponse = await likeDataLoader.LoadAsync(Id, CancellationToken.None);

        return likeResponse.Count();
    }
}