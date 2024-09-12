using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class User : IdentityUser
    {
        [Required]
        public override string UserName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public ICollection<Deck>? Decks { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<PostRating>? PostRatings { get; set; }
        public ICollection<ReplyRating>? ReplyRatings { get; set; }

    }
}
