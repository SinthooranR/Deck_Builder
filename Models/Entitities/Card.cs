using System.ComponentModel.DataAnnotations.Schema;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Card
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public string UserId { get; set; }

        public int UrlId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string FrameType { get; set; }

        public string Type { get; set; }

        public string Race { get; set; }

        public string Attribute { get; set; }

        public int Attack {  get; set; }

        public int Defense { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ShopUrl { get; set; }

        public Deck Deck { get; set; }

        public User User { get; set; }
    }
}
