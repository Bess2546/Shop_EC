using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class AddToCartRequest
    {
        [Required(ErrorMessage = "กรุณาระบุตัวสินค้า")]
        public int? ProductId {get; set;}

        [Required(ErrorMessage = "กรุณาระบุจำนวน")]
        [Range(1, 999, ErrorMessage = "จำนวนต้องอยู่ระหว่าง 1-999 ชิ้น")]
        public int? Quantity { get; set; }
    }
}