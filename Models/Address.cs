using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
        public int DistrictId { get; set; }
        public District? District { get; set; }
        public int SubDistrictId { get; set; }
        public SubDistrict? SubDistrict { get; set; }
        public string Detail { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
    }
}