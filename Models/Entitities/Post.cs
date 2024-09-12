using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public ICollection<Reply>? Replies { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public ICollection<PostRating>? PostRatings { get; set; }
    }

    public class PostRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }
        public bool? IsThumbsUp { get; set; }
    }
}
