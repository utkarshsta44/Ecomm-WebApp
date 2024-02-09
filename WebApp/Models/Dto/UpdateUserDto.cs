using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WebApp.Models.Domain;

namespace WebApp.Models.Dto
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
     
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        public string? PhoneNo { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }


        public DateTime CreatedAt { get; set; }
        public Boolean IsActive { get; set; }

    }
}