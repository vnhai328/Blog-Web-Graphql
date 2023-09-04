using auth_graphql.Models;

namespace auth_graphql.Schema.Mutations.Users
{
    public record class SignUpPayLoad(User user, string? clientMutationId);
}