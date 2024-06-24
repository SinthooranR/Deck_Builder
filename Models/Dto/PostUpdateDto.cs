namespace YTCG_Deck_Builder_API.Models.Dto
{
    public class PostUpdateDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }

    public class PostRatingUpdateDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
        public bool? IsThumbsUp { get; set; }
    }
}
