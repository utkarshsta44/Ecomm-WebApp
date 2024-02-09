using System.ComponentModel;

namespace WebApp.Models.Dto
{
    public class CartWithProduct
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int CartItems { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public float ProductPrice { get; set; }
        public string ProductImage { get; set; }

        [DefaultValue(true)]
        public bool IsAvailable { get; set; }
        public bool IsFavourite { get; set; }

        public bool IsTrending { get; set; }
        public bool IsActive { get; set; }
        public DateTime ProductCreatedAt { get; set; }
    }
}
