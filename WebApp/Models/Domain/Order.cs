using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Domain
{
    public class Order
    {
        [Key]
        public int Id { get; set; } 
        public string Email { get; set; }
        public string UserId {  get; set; }
        [ForeignKey(nameof(UserId))]
        public UserModel UserModel {  get; set; }   
        public List<OrderItems> OrderItems { get; set; }
    }
}