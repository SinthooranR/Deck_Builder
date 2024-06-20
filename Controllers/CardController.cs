using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YTCG_Deck_Builder_API.Data;
using YTCG_Deck_Builder_API.Models.Dto;
using YTCG_Deck_Builder_API.Models.Entitities;

namespace YTCG_Deck_Builder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeckController> _logger;

        public CardController(DataContext dataContext, ILogger<DeckController> logger, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> getAllCardsFromUser([FromQuery] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var cards = _dataContext.Cards.Select(c => new CardAddDto()
            {
                UrlId = c.UrlId,
                Name = c.Name,
                Level = c.Level,
                FrameType = c.FrameType,
                Type = c.Type,
                Race = c.Race,
                Attribute = c.Attribute,
                Attack = c.Attack,
                Defense = c.Defense,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                ShopUrl = c.ShopUrl,
            });
            return Ok(cards);
        }

        [HttpPut]
        public async Task<IActionResult> updateCardsInDeck([FromQuery] int deckId, [FromQuery] string userdId, [FromBody] List<CardAddDto> cards)
        {
            var deck = _dataContext.Decks.Include(d => d.Cards).Where(d => d.Id == deckId && d.UserId.Equals(userdId)).FirstOrDefault();
            if (deck == null)
            {
                return NotFound();
            }

            _dataContext.Cards.RemoveRange(deck.Cards);

            var newCards = cards.Select(c => new Card
            {
                UrlId = c.UrlId,
                Name = c.Name,
                Level = c.Level,
                FrameType = c.FrameType,
                Type = c.Type,
                Race = c.Race,
                Attribute = c.Attribute,
                Attack = c.Attack,
                Defense = c.Defense,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                ShopUrl = c.ShopUrl,
                DeckId = deckId,
                UserId = userdId
            }).ToList();

            deck.Cards.Clear();

            deck.Cards = newCards;
            _dataContext.Update(deck);
            _dataContext.SaveChanges();
            return Ok(deck);
        }
    }
}
