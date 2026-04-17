using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class CreateStoreRequest
    {
        [Required(ErrorMessage = "กรุณาระบุชื่อร้าน")]
        [MaxLength(100, ErrorMessage = "ชื่อร้านต้องไม่เกิน 100 ตัวอักษร")]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500, ErrorMessage = "คำอธิบายต้องไม่เกิน 500 ตัวอักษร")]
        public string? Description { get; set; }
    }
}