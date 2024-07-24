using Microsoft.AspNetCore.SignalR;
using YTCG_Deck_Builder_API.Models.Dto;

namespace YTCG_Deck_Builder_API.Hubs

{
    public class PostRatingHub : Hub
    {
        public async Task UpdateRating(PostRatingUpdateDto postRatingUpdateDto)
        {
            await Clients.All.SendAsync("ReceiveRatingUpdate", postRatingUpdateDto);
        }
    }
}
