using System.ComponentModel.DataAnnotations.Schema;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Card> Cards { get; set; }
    }

}
