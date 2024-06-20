namespace YTCG_Deck_Builder_API.Models.Dto
{
    public class ReplyUpdateDto
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public int ReplyId { get; set; }
    }
}
