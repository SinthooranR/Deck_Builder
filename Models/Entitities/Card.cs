using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DeckId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int UrlId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Level { get; set; }

        public string FrameType { get; set; }

        public string Type { get; set; }

        public string Race { get; set; }

        public string Attribute { get; set; }

        public int Atk { get; set; }

        public int Def { get; set; }

        public string Desc { get; set; }

        public string ImageUrl { get; set; }

        public string ShopUrl { get; set; }

        [ForeignKey(nameof(DeckId))]
        public Deck Deck { get; set; }

    }
}
