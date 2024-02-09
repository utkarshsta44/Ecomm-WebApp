using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Domain
{
    public class OrderItems
    {
        public int Id { get; set; } 
        public int Amount { get; set; }

        public int ProductId {  get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }    
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
