using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.Models;

namespace Shop_Backend.DTOs
{
    public class CartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal TotalPrice {get; set; }
    }
}