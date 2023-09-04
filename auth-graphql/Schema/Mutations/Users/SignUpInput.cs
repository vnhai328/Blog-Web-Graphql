namespace auth_graphql.Schema.Mutations.Users;

public class SignUpInput
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Uri? image { get; set; }
    public string? ClientMutationId { get; set; }
}