using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "กรุณาระบุชื่อสินค้า")]
        [MaxLength(100, ErrorMessage = "ชื่อสินค้าต้องไม่เกิน 100 ตัวอักษร")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาระบุราคา")]
        [Range(0.01, 9999999, ErrorMessage = "ราคาต้องอยู่ระหว่าง 0.01 - 9,999,999")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "กรุณาระบุจำนวนคงเหลือ")]
        [Range(0, int.MaxValue, ErrorMessage = "จำนวนต้องไม่ติดลบ")]
        public int? Stock { get; set; }

        [Required(ErrorMessage = "กรุณาระบุร้านค้า")]
        public int? StoreId { get; set; }
        
        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }
}