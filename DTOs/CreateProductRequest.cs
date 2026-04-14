using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class CreateProductRequest
    {
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999999)]
        public decimal Price { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public int StoreId { get; set; }
           
    }
}