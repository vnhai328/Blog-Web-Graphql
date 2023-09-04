using auth_graphql.DataLoaders;
using auth_graphql.Models;
using auth_graphql.Schema.Queries.People;

namespace auth_graphql.Schema.Queries.Likes;

public class LikeType
{
    public int Id { get; set; }
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
    public DateTimeOffset DateLiked { get; set; }
    public int Like_Id { get; set; }
    public Like_Type Like_Type { get; set; }
}