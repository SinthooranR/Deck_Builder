namespace YTCG_Deck_Builder_API.Models.Dto
{
    public class CardAddDto
    {
        public int UrlId { get; set; }

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
    }
}
