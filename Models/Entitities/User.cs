using Microsoft.AspNetCore.Identity;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class User : IdentityUser
    {
        public override string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Deck>? Decks { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Reply>? Replies { get; set; }
        public ICollection<PostRating>? PostRatings { get; set; }
        public ICollection<ReplyRating>? ReplyRatings { get; set; }

    }
}
