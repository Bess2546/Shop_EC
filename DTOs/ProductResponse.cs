using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StoreId { get; set; }
    }
}