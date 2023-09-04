namespace auth_graphql.Schema.Queries.People;

public class PersonType : ISearchResultType
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTimeOffset LastSeen { get; set; }
    public Uri? ImageUri { get; set; }
}