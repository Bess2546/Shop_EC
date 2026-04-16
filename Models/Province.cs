using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.Models
{
    public class Province
    {
        public int Id { get; set; }
        
        public string NameTh { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public ICollection<District> Districts { get; set; } = new List<District>();
    }
}