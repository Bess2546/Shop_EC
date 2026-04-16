using Microsoft.AspNetCore.Mvc;
using Shop_Backend.Data;
using Shop_Backend.Models;
using System.Text.Json;

namespace Shop_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public SeedController(AppDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        [HttpPost("locations")]
        public async Task<IActionResult> SeedLocations()
        {
            if (_context.Provinces.Any())
                return BadRequest(new { error = "ข้อมูลมีอยู่แล้ว" });

            var baseUrl = "https://raw.githubusercontent.com/kongvut/thai-province-data/refs/heads/master/api/latest";

            // 1. Import จังหวัด
            var provinceJson = await _httpClient.GetStringAsync($"{baseUrl}/province.json");
            var provinces = JsonSerializer.Deserialize<List<JsonElement>>(provinceJson)!;

            foreach (var p in provinces)
            {
                _context.Provinces.Add(new Province
                {
                    Id = p.GetProperty("id").GetInt32(),
                    NameTh = p.GetProperty("name_th").GetString()!,
                    NameEn = p.GetProperty("name_en").GetString()!
                });
            }
            await _context.SaveChangesAsync();

            // 2. Import อำเภอ
            var districtJson = await _httpClient.GetStringAsync($"{baseUrl}/district.json");
            var districts = JsonSerializer.Deserialize<List<JsonElement>>(districtJson)!;

            foreach (var d in districts)
            {
                _context.Districts.Add(new District
                {
                    Id = d.GetProperty("id").GetInt32(),
                    NameTh = d.GetProperty("name_th").GetString()!,
                    NameEn = d.GetProperty("name_en").GetString()!,
                    ProvinceId = d.GetProperty("province_id").GetInt32()
                });
            }
            await _context.SaveChangesAsync();

            // 3. Import ตำบล
            var subDistrictJson = await _httpClient.GetStringAsync($"{baseUrl}/sub_district.json");
            var subDistricts = JsonSerializer.Deserialize<List<JsonElement>>(subDistrictJson)!;

            foreach (var s in subDistricts)
            {
                _context.SubDistricts.Add(new SubDistrict
                {
                    Id = s.GetProperty("id").GetInt32(),
                    NameTh = s.GetProperty("name_th").GetString()!,
                    NameEn = s.GetProperty("name_en").GetString()!,
                    ZipCode = s.GetProperty("zip_code").GetInt32(),
                    DistrictId = s.GetProperty("district_id").GetInt32()
                });
            }
            await _context.SaveChangesAsync();

            return Ok(new
            {
                provinces = provinces.Count,
                districts = districts.Count,
                subDistricts = subDistricts.Count
            });
        }
    }
}