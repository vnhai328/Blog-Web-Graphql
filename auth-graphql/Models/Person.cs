namespace auth_graphql.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public Uri? ImageUri { get; set; }
    }
}