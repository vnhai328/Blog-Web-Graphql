namespace auth_graphql.Models;

public class Post
{
    public int Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public string Access { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Comment> Comments { get; set; }
}