using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Backend.Data;

namespace Shop_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProvinceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var provinces = await _context.Provinces
                .OrderBy(p => p.NameTh)
                .Select(p => new { p.Id, p.NameTh, p.NameEn })
                .ToListAsync();
            return Ok(provinces);
        }

        [HttpGet("{id}/districts")]
        public async Task<IActionResult> GetDistricts(int id)
        {
            var districts = await _context.Districts
                .Where(d => d.ProvinceId == id)
                .OrderBy(d => d.NameTh)
                .Select(d => new { d.Id, d.NameTh, d.NameEn })
                .ToListAsync();
            return Ok(districts);
        }

        [HttpGet("districts/{districtId}/subdistricts")]
        public async Task<IActionResult> GetSubDistricts(int districtId)
        {
            var subDistricts = await _context.SubDistricts
                .Where(s => s.DistrictId == districtId)
                .OrderBy(s => s.NameTh)
                .Select(s => new { s.Id, s.NameTh, s.NameEn, s.ZipCode })
                .ToListAsync();
            return Ok(subDistricts);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var provinces = await _context.Provinces
                .Where(p => p.NameTh.Contains(q) || p.NameEn.Contains(q))
                .OrderBy(p => p.NameTh)
                .Select(p => new { p.Id, p.NameTh, p.NameEn })
                .ToListAsync();
            return Ok(provinces);
        }
    }
}