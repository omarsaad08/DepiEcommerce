using System.ComponentModel.DataAnnotations;

namespace DepiEcommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string Image { get; set; }
        [Required]
        [Display(Name = " Product Quantity")]
        public int ProductsInStock { get; set; }
    }
}
