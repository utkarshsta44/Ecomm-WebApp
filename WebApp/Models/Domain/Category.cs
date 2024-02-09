using WebApp.Models.Domain;

namespace WebApp.Models.Domain
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}