namespace WebApp.Models.Dto
{
    public class Favourites
    {
        public int FId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public float ProductPrice { get; set; }
        public string ProductImage { get; set; }
    }
}
