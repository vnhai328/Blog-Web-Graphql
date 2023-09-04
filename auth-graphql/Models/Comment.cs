using System.ComponentModel.DataAnnotations;

namespace auth_graphql.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public Guid CreatorId { get; set; }
        public string CommentText { get; set; }
        public DateTimeOffset DateCommented { get; set; } = DateTimeOffset.UtcNow;
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}