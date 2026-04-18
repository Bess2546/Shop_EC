using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "กรุณาเลือกสินค้าอย่างน้อย 1 รายการ")]
        [MinLength(1, ErrorMessage = "ต้องเลือกสินค้าอย่างน้อย 1 รายการ")]
        public List<int> CartItemIds { get; set; } = new();
    }
}