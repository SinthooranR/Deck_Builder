namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Reply
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public Post? Post { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<ReplyRating>? ReplyRatings { get; set; }
    }

    public class ReplyRating
    {
        public int Id { get; set; }
        public int ReplyId { get; set; }
        public Reply Reply { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public bool IsThumbsUp { get; set; }
    }
}
