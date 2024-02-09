
using WebApp.Models.Domain;


namespace WebApp.Models.Domain
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CartItems { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string UserId1 { get; set; }
        public UserModel User { get; set; }

        public Product Product { get; set; }
    }
}