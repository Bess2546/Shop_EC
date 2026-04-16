using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.Models
{
    public class SubDistrict
    {
        public int Id { get; set; }
        public string NameTh { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public int DistrictId { get; set; }
        public District? District { get; set; }
    }
}