using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Domain
{
    public class Favourite
    {
        public int FId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UsersId { get; set; }
        public virtual Product Products { get; set; }
        public virtual UserModel Users { get; set; }
    }
}
