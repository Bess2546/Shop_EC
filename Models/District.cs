using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.Models
{
    public class District
    {
        public int Id { get; set; }
        public string NameTh { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
        public ICollection<SubDistrict> SubDistricts { get; set; } = new List<SubDistrict>();
    }
}