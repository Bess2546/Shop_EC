using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.DTOs
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
        public decimal TotalPrice {get; set; }
    }
}