namespace auth_graphql.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}