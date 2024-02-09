using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Dto
{
    public class AddProductDto
    {


        [Required]
        public string ProductName { get; set; }

        [Required]

        public string ProductDescription { get; set; }

        [Required]

        public float ProductPrice { get; set; }
        public IFormFile? ProductImage { get; set; }

        [DefaultValue(true)]
        public bool IsAvailable { get; set; }

        public bool IsTrending { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public DateTime ProductCreatedAt { get; set; }
        public int ProductCategoryId { get; set; }

    }
}