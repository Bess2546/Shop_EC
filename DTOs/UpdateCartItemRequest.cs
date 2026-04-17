using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shop_Backend.DTOs
{
    public class UpdateCartItemRequest
    {
        [Required(ErrorMessage = "กรุณาระบุจำนวน")]
        [Range(1, 999, ErrorMessage = "จำนวนต้องอยู่ระหว่าง 1-999 ชิ้น")]
        public int? Quantity { get; set; }
    }
}