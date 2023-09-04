namespace auth_graphql.Schema.Mutations.Users
{
    public class LoginInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ClientMutationId { get; set; }
    }
}