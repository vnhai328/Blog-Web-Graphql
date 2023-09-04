using auth_graphql.Models;

namespace auth_graphql.Schema.Mutations.Users
{
    public class LoginPayLoad
    {
        public LoginPayLoad(Person me, string token, string schema, string? clientMutationId)
        {
            Me = me;
            Token = token;
            Schema = schema;
            ClientMutationId = clientMutationId;
        }
        public Person Me { get; }
        public string Token { get; }
        public string Schema { get; }
        public string? ClientMutationId { get; }
    }
}