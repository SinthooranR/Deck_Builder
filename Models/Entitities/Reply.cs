using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Reply
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<ReplyRating>? ReplyRatings { get; set; }
    }

    public class ReplyRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReplyId { get; set; }

        [ForeignKey(nameof(ReplyId))]
        public Reply Reply { get; set; }

        [Required]
        public string UserId { get; set; }
        public bool? IsThumbsUp { get; set; }
    }
}
