namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public ICollection<PostRating>? PostRatings { get; set; }
    }

    public class PostRating
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public bool? IsThumbsUp { get; set; }
    }
}
