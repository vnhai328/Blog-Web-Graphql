using auth_graphql.DataLoaders;
using auth_graphql.Models;
using auth_graphql.Schema.Queries.Likes;
using auth_graphql.Schema.Queries.People;
using HotChocolate.Resolvers;

namespace auth_graphql.Schema.Queries.Comments;

public class CommentType
{
    public int Id { get; set; }
    [IsProjected(true)]
    public Guid CreatorId { get; set; }
     public async Task<PersonType> Creator([Service] PersonByUserIdDataLoader personByUserIdData)
    {
        var person = await personByUserIdData.LoadAsync(CreatorId, CancellationToken.None);
        if(person is null)
        {
            throw new GraphQLException(new Error("Some things wentwrong","ERROR_404"));
        }
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
    public string CommentText { get; set; }
    public DateTimeOffset DateCommented { get; set; } = DateTimeOffset.UtcNow;
    [IsProjected(true)]
    public int PostId { get; set; }
    public Post Post { get; set; }
    public async Task<IEnumerable<LikeType>> CommentLikes([Service] CommentLikeDataLoader likeDataLoader)
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