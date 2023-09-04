namespace auth_graphql.Models;

public class User
{
    public Guid Id { get; set; }
    [GraphQLIgnore]
    public Guid PersonId1 { get; set; }
    public Person Person { get; set; }
    public string Email { get; set; }
    [GraphQLIgnore]
    public string PassWordHash { get; set; }
    [GraphQLIgnore]
    public string Salt { get; set; }
}