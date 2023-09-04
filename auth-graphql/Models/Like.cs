namespace auth_graphql.Models;

public class Like
{
    public int Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTimeOffset DateLiked { get; set; }
    public int Like_Id { get; set; }
    public Like_Type Like_Type {get; set; }
}

public enum Like_Type 
{
    Post,
    Comment
}