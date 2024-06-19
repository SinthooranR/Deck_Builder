using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YTCG_Deck_Builder_API.Models.Entitities
{
    public class User : IdentityUser
    {
        public override string UserName { get; set; }
        public DateTime CreatedAt { get; set; }

        //#TODO Need to Create Deck Entity
        public ICollection<Deck>? Decks { get; set; }

        public ICollection<Card>? Cards { get; set; }



    }
}
